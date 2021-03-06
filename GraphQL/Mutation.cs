namespace hot_chocolate_demo.GraphQL;

public class Mutation {
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