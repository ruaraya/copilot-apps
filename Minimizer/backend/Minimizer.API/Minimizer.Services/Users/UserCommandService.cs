using MediatR;
using Microsoft.EntityFrameworkCore;
using Minimizer.Common.Models.Users;
using Minimizer.Data.DbContexts;
using Minimizer.Entities.Models;

namespace Minimizer.Services.Users
{
    public class UserCommandService :
        IRequestHandler<InsertUserCommand, UserResponse>,
        IRequestHandler<UpdateUserCommand, UserResponse>,
        IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly MinimizerDbContext context;

        public UserCommandService(MinimizerDbContext context)
        {
            this.context = context;
        }

        #region Handlers

        public async Task<UserResponse> Handle(InsertUserCommand request,
            CancellationToken cancellationToken) =>

            await InsertUserAsync(request.UpsertUserRequest, cancellationToken);

        public async Task<UserResponse> Handle(UpdateUserCommand request,
            CancellationToken cancellationToken) =>

            await UpdateUserAsync(request.UserId, request.UpsertUserRequest, cancellationToken);

        public async Task<Unit> Handle(DeleteUserCommand request,
            CancellationToken cancellationToken) =>

            await DeleteUserAsync(request.UserId, cancellationToken);

        #endregion Handlers

        #region Public methods

        public async Task<UserResponse> InsertUserAsync(UpsertUserRequest upsertUser, CancellationToken cancellationToken)
        {
            User newUser = await InsertUserEntityAsync(upsertUser, cancellationToken);

            return new UserResponse() { UserName = newUser.Username, Role = newUser.Role, PasswordHash = newUser.PasswordHash };
        }

        public async Task<UserResponse> UpdateUserAsync(Guid userId, UpsertUserRequest upsertUser, CancellationToken cancellationToken)
        {
            User updatedUser = await UpdateUserEntityAsync(userId, upsertUser, cancellationToken);

            return new UserResponse() { UserName = updatedUser.Username, Role = updatedUser.Role, PasswordHash = updatedUser.PasswordHash };
        }

        public async Task<Unit> DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            await DeleteUserEntityAsync(userId, cancellationToken);

            return default;
        }

        #endregion Public methods

        #region Private methods

        private async Task<User> InsertUserEntityAsync(UpsertUserRequest upsertUser, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = upsertUser.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(upsertUser.Password),
                Role = upsertUser.Role
            };
            
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync(cancellationToken);

            return newUser;
        }

        private async Task<User> UpdateUserEntityAsync(Guid userId, UpsertUserRequest upsertUser, CancellationToken cancellationToken)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            user.Username = upsertUser.UserName;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(upsertUser.Password);
            user.Role = upsertUser.Role;
            await context.SaveChangesAsync(cancellationToken);

            return user;
        }

        private async Task<Unit> DeleteUserEntityAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null) 
                return default;

            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
            return default;
        }

        #endregion Private methods

    }

    #region Commands

    public record InsertUserCommand(UpsertUserRequest UpsertUserRequest) : IRequest<UserResponse>;
    public record UpdateUserCommand(Guid UserId, UpsertUserRequest UpsertUserRequest) : IRequest<UserResponse>;
    public record DeleteUserCommand(Guid UserId) : IRequest<Unit>;

    #endregion Commands
}
