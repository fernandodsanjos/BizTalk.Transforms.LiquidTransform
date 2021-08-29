namespace TestLiquid
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    
    public class Generated 
    {
        [JsonProperty("_id")]
        public string Id {get; set;}
        [JsonProperty("index")]
        public int Index {get; set;}
        [JsonProperty("guid")]
        public string Guid {get; set;}
        [JsonProperty("isActive")]
        public bool IsActive {get; set;}
        [JsonProperty("balance")]
        public string Balance {get; set;}
        [JsonProperty("picture")]
        public string Picture {get; set;}
        [JsonProperty("age")]
        public int Age {get; set;}
        [JsonProperty("eyeColor")]
        public string EyeColor {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}
        [JsonProperty("gender")]
        public string Gender {get; set;}
        [JsonProperty("company")]
        public string Company {get; set;}
        [JsonProperty("email")]
        public string Email {get; set;}
        [JsonProperty("phone")]
        public string Phone {get; set;}
        [JsonProperty("address")]
        public string Address {get; set;}
        [JsonProperty("about")]
        public string About {get; set;}
        [JsonProperty("registered")]
        public string Registered {get; set;}
        [JsonProperty("latitude")]
        public float Latitude {get; set;}
        [JsonProperty("longitude")]
        public float Longitude {get; set;}
        [JsonProperty("tags")]
        public IList<string> Tags {get; set;}
        [JsonProperty("friends")]
        public IList<Friends> Friends {get; set;}
        [JsonProperty("greeting")]
        public string Greeting {get; set;}
        [JsonProperty("favoriteFruit")]
        public string FavoriteFruit {get; set;}
    }

    public class Friends 
    {
        [JsonProperty("id")]
        public int Id {get; set;}
        [JsonProperty("name")]
        public string Name {get; set;}
    }
}
