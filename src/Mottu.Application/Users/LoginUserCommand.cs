using MediatR;

namespace Mottu.Application.Users.Commands
{
    public class LoginUserCommand : IRequest<string>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
