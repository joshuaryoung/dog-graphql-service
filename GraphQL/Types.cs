using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace hot_chocolate_demo.GraphQL;

public class Dog {
  [JsonPropertyName("id")]
  [Column("id")]
  public string? Id { get; set; }
  [JsonPropertyName("breeds")]
  [Column("breed")]
  public Breed[]? Breed { get; set; }
  [JsonPropertyName("url")]
  [Column("avatar_url")]
  public string? AvatarUrl { get; set; }
}

public class Breed {
  [JsonPropertyName("name")]
  [Key]
  public string? Name { get; set; }
}

public class User { 
  [Column("id")]
  public int Id { get; set; }

  [Column("dogs")]
  public List<string>? DogsIdList { get; set; }

  [Column("first_name")]
  public string? FirstName { get; set; }

  [Column("last_name")]
  public string? LastName { get; set; }

  [Column("avatar_url")]
  public string? AvatarUrl { get; set; }

  [Column("username")]
  public string? Username { get; set; }

  [Column("password")]
  public string? Password { get; set; }

  public List<Dog> GetDogs(DogDataContext dbContext, [Parent] User currentUser, int page = 0, int pageSize = 10) {
    int pageParam = page;
    if (page < 0) pageParam = 0;
    if (pageSize < 1) pageSize = 1;
    var userDogs = dbContext.dog.Where(dog => currentUser.DogsIdList.Contains(dog.Id)).Skip(pageParam * pageSize).Take(pageSize).ToList();
    return userDogs;
  }
}

public class GetDogsRes {
  public List<Dog>? Data { get; set; }
  public int? TotalResults { get; set; }
}

public class QueryRes<T> {
  public T? Data { get; set; }
  public int? TotalResults { get; set; }
}

public class MutationRes<T> {
  public T? Data { get; set; }
}

public class AuthPayload {
  public bool Success { get; set; }
  public string? JWT { get; set; }
  public string? Message { get; set; }
}