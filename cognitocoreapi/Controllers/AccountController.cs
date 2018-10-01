using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using cognitocoreapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace cognitocoreapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private AmazonCognitoIdentityProviderClient _client;
        private IConfiguration _configuration;

        private readonly string _clientId;
        private readonly string _poolId;
        private readonly CognitoUserPool _userPool;

        public AccountController(IConfiguration configuration)
        {
            _client = new AmazonCognitoIdentityProviderClient(RegionEndpoint.EUCentral1);
            _configuration = configuration;
            _clientId = _configuration["AWS:ClientId"];
            _poolId = _configuration["AWS:UserPoolId"];
            _userPool = new CognitoUserPool(poolID: _poolId, clientID: _clientId, provider: _client);
        }

        // POST api/account
        [HttpPost]
        public async Task Register([FromBody] RegisterModel model)
        {
            try
            {
                var signUpRequest = new SignUpRequest()
                {
                    ClientId = _clientId,
                    Password = model.Password,
                    Username = model.Email,
                };

                var attributes = new List<AttributeType>(){
                    new AttributeType(){Name="email",Value=model.Email},
                    new AttributeType(){Name="name",Value=model.FirstName},
                    new AttributeType(){Name="family_name",Value=model.LastName},
                    new AttributeType(){Name="phone_number",Value=model.PhoneNumber}
                };
                foreach (var attribute in attributes)
                {
                    signUpRequest.UserAttributes.Add(attribute);
                }
                var signUpResponse = await _client.SignUpAsync(signUpRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            try
            {
                var user = new CognitoUser(userID: model.Email, clientID: _clientId, pool: _userPool, provider: _client, username: model.Email);

                AuthFlowResponse context = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest()
                {
                    Password = model.Password
                }).ConfigureAwait(false);

                var userResponse = await _client.AdminGetUserAsync(new AdminGetUserRequest()
                {
                    Username = user.UserID,
                    UserPoolId = _userPool.PoolID
                }).ConfigureAwait(false);


                return new OkObjectResult(new 
                {
                    metadata = new
                    {
                        token = context.AuthenticationResult.IdToken,
                        userName = user.Username
                    }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [HttpGet("{token}")]
        [AllowAnonymous]
        public async Task Activate(string token)
        {
            try
            {
                var confirmRequest = new ConfirmSignUpRequest()
                {
                    ClientId = _clientId,
                    ConfirmationCode = token,
                };

                var confirmResult = await _client.ConfirmSignUpAsync(confirmRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Reset([FromBody]ChangePasswordModel model)
        {
            try
            {
                var resetPasswordRequest = new ChangePasswordRequest()
                {
                    AccessToken = model.Token,
                    PreviousPassword = model.OldPassword,
                    ProposedPassword = model.NewPassword
                };

                var resetPasswordResponse = await _client.ChangePasswordAsync(resetPasswordRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Forgot([FromBody]ForgotPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new InvalidOperationException();
                }

                var forgotPasswordRequest = new ForgotPasswordRequest()
                {
                    ClientId = _clientId,
                    Username = model.Email
                };

                var forgotPasswordResponse = await _client.ForgotPasswordAsync(forgotPasswordRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}