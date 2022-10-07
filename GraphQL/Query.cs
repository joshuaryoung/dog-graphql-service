namespace hot_chocolate_demo.GraphQL;
using System.Security.Claims;
using HotChocolate.AspNetCore.Authorization;

public class Query {
  public GetDogsRes GetDogs(ClaimsPrincipal claimsPrincipal, DogDataContext dbContext, string? idIn) {

    List<Dog> res = FetchDogs(dbContext);

    return new GetDogsRes() {
      Data = res,
      TotalResults = res.Count()
    };
  }

  public List<User>? GetUsers(ClaimsPrincipal claimsPrincipal, DogDataContext dbContext) {
    return dbContext?.user?.ToList();
  }

  public QueryRes<List<Dog>> GetUserDogs(ClaimsPrincipal claimsPrincipal, DogDataContext dbc, int? idIn, int page = 0, int pageSize = 10) {
    var role = claimsPrincipal?.Claims.SingleOrDefault(claim => claim.Type.Contains("role"))?.Value;
    var claimId = claimsPrincipal?.Claims.SingleOrDefault(claim => claim.Type == "id")?.Value;

    if (claimId == null) {
      throw new Exception("Invalid JWT received!");
    }
    var idParam = role == "admin" && idIn != null ? idIn : int.Parse(claimId);
    var currentUser = dbc?.user?.First(user => user.Id == idParam);
    var dogs = currentUser?.GetDogs(dbc!, currentUser, page, pageSize);
    var returnObj = new QueryRes<List<Dog>>() { Data = dogs, TotalResults = currentUser?.DogsIdList?.Count() };
    return returnObj;
  }

  public QueryRes<List<Dog>> GetMyDogs(ClaimsPrincipal claimsPrincipal, DogDataContext dbc, int page = 0, int pageSize = 10) {
    var claimId = claimsPrincipal?.Claims.SingleOrDefault(claim => claim.Type == "id")?.Value;

    if (claimId == null) {
      throw new Exception("Invalid JWT received!");
    }
    var idParam = int.Parse(claimId);
    var currentUser = dbc?.user?.First(user => user.Id == idParam);
    var dogs = currentUser?.GetDogs(dbc!, currentUser, page, pageSize);
    var returnObj = new QueryRes<List<Dog>>() { Data = dogs, TotalResults = currentUser?.DogsIdList?.Count() };
    return returnObj;
  }

  public User? GetMe([Service] IHttpContextAccessor httpContextAccessor, ClaimsPrincipal claimsPrincipal, DogDataContext dbContext) {
    var claimId = claimsPrincipal?.Claims.SingleOrDefault(claim => claim.Type == "id")?.Value;
    if (claimId == null) {
      throw new Exception("Invalid JWT received!");
    }

    return dbContext.user?.Where(el => el.Id == int.Parse(claimId)).ToList().First<User>();
  }

  public User? GetUserById([Service] IHttpContextAccessor httpContextAccessor, ClaimsPrincipal claimsPrincipal, DogDataContext dbContext, int? idIn) {
    var roles = claimsPrincipal?.Claims.Where(claim => claim.Type.Contains("role"))?.Select(claim => claim.Value) ?? new List<string>();
    var claimId = claimsPrincipal?.Claims.SingleOrDefault(claim => claim.Type == "id")?.Value;
    if (claimId == null) {
      throw new Exception("Invalid JWT received!");
    }
    if (!idIn.HasValue) {
      throw new ArgumentException("idIn is a required parameter!");
    }

    var isAuthorized = roles.Contains("admin") || idIn == int.Parse(claimId);

    if (!isAuthorized) {
      string msg = "Not Authorized to Access This User";
      Console.WriteLine(msg);
      throw new UnauthorizedAccessException(msg);
    }

    return dbContext.user?.Where(el => el.Id == idIn).ToList().First<User>();
  }

  private List<Dog> FetchDogs(DogDataContext dbContext) {
    List<Dog>? dbRes = new List<Dog>();
    try {
      dbRes = dbContext?.dog?.ToList();
    } catch(Exception error) {
      Console.WriteLine(error);
    }

    List<Dog> payload = new List<Dog>();
    payload.AddRange(dbRes!);

    return payload;
  }
}