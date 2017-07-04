using System;
using System.Collections.Generic;

namespace BUAnalytics{

	public enum BUGender{
		Male, Female
	}

	public class BUUser: BUDocument{
		
		public string Collection;

		public static BUUser Current;

		public string UserId = BUID.Instance.Generate();

		public string Username;
		public string Name;
		public string FirstName;
		public string LastName;
		public string Email;
		public string Phone;
		public int? Age;
		public BUGender? Gender;

		public BUUser(string collection) {
			Collection = collection;
		}

		public void Upload(){

			//Add required fields
			this.Add("userId", UserId);

			//Add optional fields
			if (Username != null){ Add("username", Username); }
			if (Name != null){ Add("name", Name); }
			if (FirstName != null){ Add("first_name", FirstName); }
			if (LastName != null){ Add("last_name", LastName); }
			if (Email != null){ Add("email", Email); }
			if (Phone != null){ Add("phone", Phone); }
			if (Age != null){ Add("age", Age); }
			if (Gender != null){ Add("gender", Gender); }

			//Add to collection manager
			BUCollectionManager.Instance.Add(Collection ?? "Users", this);

			//Remove current if self
			if (BUUser.Current == this){
				BUUser.Current = null;
			}
		}
	}
}