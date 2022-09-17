namespace hot_chocolate_demo.GraphQL;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Security.Claims;

public class Mutation {
  private readonly IConfiguration _config;
  public Mutation(IConfiguration config) {
    _config = config;
  }
  public MutationRes<AuthPayload> UserAuthenticate(DogDataContext dbContext, string username, string password) {
    User? currentUser = null;
    try {
      currentUser = dbContext?.user?.Where(el => el.Username == username).FirstOrDefault();
    } catch(Exception error) {
      Console.WriteLine(error);
      throw new Exception("Login Failed");
    }

    if (currentUser == null) {
      throw new Exception("Login Failed");
    }

    var passwordHasher = new PasswordHasher<User>();
    // var hashedPassword = passwordHasher.HashPassword(currentUser, password);
    var verifyPasswordRes = passwordHasher.VerifyHashedPassword(currentUser, currentUser.Password, password);
    var verifyPasswordResString = verifyPasswordRes.ToString();
    // Console.WriteLine("hashedPassword: " + hashedPassword);
    Console.WriteLine("password: " + password);
    Console.WriteLine("verifyPasswordResString: " + verifyPasswordResString);

    if (verifyPasswordRes == 0) {
      throw new Exception("Login Failed");
    }

    var tokenHandler = new JwtSecurityTokenHandler();
    var keyString = _config["JwtSecretKey"];
    Console.WriteLine("keyString: " + keyString);
    var key = UTF8Encoding.UTF8.GetBytes(_config["JwtSecretKey"]!);
    var Expires = DateTime.UtcNow.AddDays(1);

    var tokenDescriptor = new SecurityTokenDescriptor() {
      Subject = new ClaimsIdentity(new[] { new Claim("id", currentUser.Id.ToString()!), new Claim("username", currentUser.Username!) }),
      Expires = Expires,
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var Jwt = tokenHandler.WriteToken(token);

    return new MutationRes<AuthPayload>() { Data = new AuthPayload() { Success = true, Message = "Login Successful", Jwt = Jwt, Expires = Expires, User = currentUser } };
  }
  public bool AddDogToDbAndUserList(DogDataContext dbContext, int userIdIn, Dog dogIn) {
    try {
      AddDog(dbContext, dogIn);
      AddDogToUserList(dbContext, dogIn.Id, userIdIn);
    } catch (Exception error) {
      Console.WriteLine(error);
      return false;
    }

    return true;
  }
  public bool AddDogToUserList(DogDataContext dbContext, string dogIdIn, int userIdIn) {
    User currentUser = new User();
    
    try {
      currentUser = dbContext?.user?.Where(el => el.Id == userIdIn).FirstOrDefault() ?? new User();
    } catch(Exception error) {
      Console.WriteLine(error);
      return false;
    }   
    
    List<string> updatedDogList = new List<string>();
    if (currentUser.DogsIdList != null) {
      updatedDogList.AddRange(currentUser.DogsIdList);
    }
    if (!updatedDogList.Contains(dogIdIn)) {
      updatedDogList.Add(dogIdIn);
    }

    currentUser.DogsIdList = updatedDogList;

    dbContext?.SaveChanges();

    return true;
  }
  public bool AddDog(DogDataContext dbContext, Dog dogIn) {
    Dog dogInDb = new Dog();

    try {
      dogInDb = dbContext?.dog?.FirstOrDefault(el => el.Id == dogIn.Id);
    } catch (Exception error) {
      throw error;
    }

    if (dogInDb == null) {
      try {
        dbContext.dog.Add(dogIn);
      } catch(Exception error) {
        throw error;
      }
    } else {
      try {
        dogInDb = dogIn;
      } catch(Exception error) {
        throw error;
      }
    }


    dbContext.SaveChanges();

    return true;
  }

  public MutationRes<string> RemoveDogFromUserList(DogDataContext dbc, int userIdIn, string dogIdIn) {
    var currentUser = dbc!.user!.FirstOrDefault(el => el.Id == userIdIn);

    if (currentUser == null) {
      throw new Exception("Specified user not found!");
    }

    List<string> dogList = currentUser!.DogsIdList ?? new List<string>();
    var removed = dogList.Remove(dogIdIn);

    dbc.SaveChanges();
    return new MutationRes<string>() { Data = removed ? dogIdIn : "" };
  }
}