namespace TicTacToe.Web.Mappers;

using TicTacToe.DataSource.Models;
using TicTacToe.Web.Models;

public class DataWebMapper
{
    public static UserDAO UserToDAO(UserDTO user)
    {
        return new UserDAO { Uuid = user.Uuid, Login = user.Login, Password = user.Password };
    }
    public static UserDTO UserToDTO(UserDAO user)
    {
        return new UserDTO { Uuid = user.Uuid, Login = user.Login, Password = user.Password };
    }
}