using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AspSample.App.Main.Models
{
    public class User : IdentityUser<int>
    {
        List<Post> Posts { get; set; }
    }
}
