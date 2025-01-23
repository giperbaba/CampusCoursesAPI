using Microsoft.EntityFrameworkCore;
using repassAPI.Data;
using repassAPI.Entities;
using repassAPI.Exceptions;
using repassAPI.Models.Request;
using repassAPI.Models.Response;
using repassAPI.Services.Interfaces;
using repassAPI.Utils;

namespace repassAPI.Services.Impl;

public class AccountService: IAccountService
{
    private readonly DatabaseContext _context;
    private readonly TokenProvider _tokenProvider;
    private readonly ITokenService _tokenService;

    public AccountService(DatabaseContext context, TokenProvider tokenProvider, ITokenService tokenService)
    {
        _context = context;
        _tokenProvider = tokenProvider;
        _tokenService = tokenService;
    }

    public async Task<TokensResponse> Register(UserRegisterRequest userRegister)
    {
        if (await _context.Users.AnyAsync(u => u.Email == userRegister.Email))
        {
            throw new ConflictException(Constants.ErrorMessages.ConflictEmail);
        }
        
        if (userRegister.Password != userRegister.ConfirmPassword)
        {
            throw new BadRequestException(Constants.ErrorMessages.IncorrectConfirmPassword);
        }
        
        IsAgeCorrect(userRegister.BirthDate);
        
        var user = Mapper.MapUserFromRegisterModelToEntity(userRegister);
        
        await _context.Users.AddAsync(user);
        await _context.UserRoles.AddAsync(new UserRoles(user.Id));
        await _context.SaveChangesAsync();
        
        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken(user);
        _tokenService.SaveRefreshToken(user, refreshToken);
        
        return new TokensResponse(accessToken, refreshToken);
    }

    public async Task<TokensResponse> Login(UserLoginRequest userLogin)
    {
        var user = GetUserByEmail(userLogin.Email);
        
        if (user == null)
        {
            throw new NotFoundException(Constants.ErrorMessages.UserNotFound);
        }
        
        if (!PasswordHasher.Verify(userLogin.Password, user.Password))
        {
            throw new BadRequestException(Constants.ErrorMessages.InvalidPassword); //TODO: изменить ошибку
        }
        
        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken(user);
        _tokenService.SaveRefreshToken(user, refreshToken);

        return new TokensResponse(accessToken, refreshToken);
    }

    public async Task<AccessTokenResponse> Refresh(RefreshRequest refreshRequest)
    {
        var email = _tokenService.GetEmailFromRefreshToken(refreshRequest.RefreshToken);
        var user = await Task.Run(() => GetUserByEmail(email));

        RemoveRefreshTokenIfInvalid(user, refreshRequest);
        
        var newAccessToken = _tokenProvider.GenerateAccessToken(user);
        return new AccessTokenResponse(newAccessToken);
    }
    
    public async Task<Response> Logout(string email, string accessToken)
    {
        await _tokenService.CheckIsAccessTokenBanned(accessToken);

        var user = GetUserByEmail(email);
        _tokenService.RemoveTokens(user, accessToken);
        return new Response(null, Constants.ErrorMessages.Logout);
    }
    
    public async Task<UserProfileResponse> GetProfile(string email, string accessToken)
    {
        await _tokenService.CheckIsAccessTokenBanned(accessToken);

        var user = GetUserByEmail(email);
        var userProfile = Mapper.MapUserEntityToUserProfileModel(user);
        return userProfile;
    }

    public async Task<UserProfileResponse> EditProfile(string email, UserEditProfileRequest userEdit, string accessToken)
    {
        await _tokenService.CheckIsAccessTokenBanned(accessToken);

        var user = GetUserByEmail(email);
        IsAgeCorrect(userEdit.BirthDate);
        Edit(user, userEdit);
        await _context.SaveChangesAsync();
        var userProfile = Mapper.MapUserEntityToUserProfileModel(user);
        return userProfile;
    }
    
    public bool IsUserAdmin(string id)
    {
        return GetUserById(id).IsAdmin;
    }

    
    private User GetUserByEmail(string email)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
            throw new NotFoundException(Constants.ErrorMessages.UserNotFound);
        }
        return user;
    }
    
    public User GetUserById(string id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id.ToString() == id);
        if (user == null)
        {
            throw new NotFoundException(Constants.ErrorMessages.UserNotFound);
        }
        return user;
    }

    private void Edit(User user, UserEditProfileRequest userEdit)
    {
        user.FullName = userEdit.FullName;
        user.BirthDate = userEdit.BirthDate;
    }
    
    private void RemoveRefreshTokenIfInvalid(User user, RefreshRequest request)
    {
        if (!_tokenProvider.IsRefreshTokenValid(user, request.RefreshToken))
        {
            _tokenService.RemoveTokens(user, null);
            throw new InvalidTokenException(Constants.ErrorMessages.InvalidToken);
        }
    }
    
    private void IsAgeCorrect(DateTime birthdate)
    {
        DateTime minimumAllowedDate = DateTime.UtcNow.AddYears(-14);
        
        if (birthdate > minimumAllowedDate)
        {
            throw new BadRequestException(Constants.ErrorMessages.InvalidDate);
        }
    }
}