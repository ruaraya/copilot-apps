using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Users;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Users
{
    public class UserQueryService :
        IRequestHandler<CheckUserByUserNameQuery, bool>,
        IRequestHandler<GetAllUsersQuery, List<UserResponse>>,
        IRequestHandler<GetUserByUserIdQuery, UserResponse?>,
        IRequestHandler<GetUserByUserNameQuery, UserResponse>
    {
        private readonly MinimizerDbContext context;

        public UserQueryService(MinimizerDbContext context) 
        {
            this.context = context;
        }

        #region Handlers

        public async Task<bool> Handle(CheckUserByUserNameQuery request, CancellationToken cancellationToken) =>
            await context.Users.AnyAsync(x => x.Username == request.UserName, cancellationToken);

        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request,
            CancellationToken cancellationToken) =>

            await GetAllUsersAsync(cancellationToken);

        public async Task<UserResponse?> Handle(GetUserByUserIdQuery request,
            CancellationToken cancellationToken) =>

            await GetUserByUserIdAsync(request.UserId, cancellationToken);

        public async Task<UserResponse> Handle(GetUserByUserNameQuery request,
            CancellationToken cancellationToken) =>

            await GetUserByUserNameAsync(request.UserName, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<List<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            var userEntities = await GetAllUserEntitiesAsync(cancellationToken);

            if (!userEntities.Any())
                return new List<UserResponse>();

            var users = userEntities.Select(u => new UserResponse
            {
                UserName = u.Username,
                Role = u.Role,
                PasswordHash = u.PasswordHash
            }).ToList();

            return users;
        }

        public async Task<UserResponse?> GetUserByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var userEntity = await GetUserEntityByUserIdAsync(userId, cancellationToken);

            if (userEntity == null)
                return null;

            return new UserResponse() { UserName = userEntity.Username, Role = userEntity.Role, PasswordHash = userEntity.PasswordHash };
        }

        public async Task<UserResponse> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var userEntity = await GetUserEntityByUserNameAsync(userName, cancellationToken);

            return new UserResponse() { UserName = userEntity.Username, Role = userEntity.Role, PasswordHash = userEntity.PasswordHash };
        }

        #endregion Public methods

        #region Private methods

        private async Task<List<User>> GetAllUserEntitiesAsync(CancellationToken cancellationToken)
        {
            return await context.Users.ToListAsync(cancellationToken);
        }

        private async Task<User?> GetUserEntityByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }

        private async Task<User> GetUserEntityByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            var userEntity = await context.Users.FirstOrDefaultAsync(x => x.Username == userName, cancellationToken);

            return userEntity == null ? new User() : userEntity;
        }

        #endregion Private methods

    }

    #region Queries
    
    public record CheckUserByUserNameQuery(string UserName) : IRequest<bool>;
    public record GetAllUsersQuery() : IRequest<List<UserResponse>>;
    public record GetUserByUserIdQuery(Guid UserId) : IRequest<UserResponse>;
    public record GetUserByUserNameQuery(string UserName) : IRequest<UserResponse>;
    
    #endregion Queries
}
