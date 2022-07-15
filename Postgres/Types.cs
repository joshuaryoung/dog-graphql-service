namespace Postgres.Db;
using System.ComponentModel.DataAnnotations.Schema;

public class DogDb {
  public string? id { get; set; } 
  [Column("avatar_url")]
  public string? avatarUrl { get; set; }
  [Column("breed")] 
  public string? breed { get; set; } 
}

// public class BreedDb {
//   [Key]
//   public string? name { get; set; }
// }
// public class BreedType : ObjectType<BreedDb>
// {
//     protected override void Configure(IObjectTypeDescriptor<BreedDb> descriptor)
//     {

//     }
// }


// public class UserDb {
//   [Column("id")]
//   public int id { get; set; }
//   [Column("dogs")]
//   public List<DogDb>? dogs { get; set; }
// }
// public class UserType : ObjectType<UserDb>
// {
//     protected override void Configure(IObjectTypeDescriptor<UserDb> descriptor)
//     {

//     }
// }
