﻿using System.Collections.Generic;

namespace MiniORM
{
    internal class ChangeTracker<T> where T : class, new()
    {
        private readonly List<T> allEntities;
        private readonly List<T> added;
        private readonly List<T> removed;

        public ChangeTracker(IEnumerable<T> entities)
        {
            this.added = new List<T>();
            this.removed = new List<T>();

            this.allEntities = new 
        }
    }
}