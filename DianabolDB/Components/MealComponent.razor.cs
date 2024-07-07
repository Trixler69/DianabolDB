using DataAccess.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianabolDB.Components
{
    public partial class MealComponent
    {
        [Parameter]
        public Meal Meal { get; set; }
    }
}
