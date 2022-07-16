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

  public List<Dog> GetDogs(DogDataContext dbContext, [Parent] User currentUser) {
    var userDogs = dbContext.dog.Where(dog => currentUser.DogsIdList.Contains(dog.Id)).ToList();
    return userDogs;
  }
}