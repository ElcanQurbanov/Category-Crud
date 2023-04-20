﻿using Microsoft.Build.Framework;

namespace Entity_Framework_Slider.Models
{
    public class Category:BaseEntity
    {
        public string? Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
