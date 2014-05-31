﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core
{
    public sealed class CakeTask
    {
        private readonly string _name;
        private readonly List<string> _dependencies;
        private readonly List<Func<ICakeContext, bool>> _criterias;
        private readonly List<Action<ICakeContext>> _actions;

        public string Name
        {
            get { return _name; }
        }

        public IReadOnlyList<string> Dependencies
        {
            get { return _dependencies; }
        }

        public IReadOnlyList<Func<ICakeContext, bool>> Criterias
        {
            get { return _criterias; }
        }

        public IReadOnlyList<Action<ICakeContext>> Actions
        {
            get { return _actions; }
        }

        public CakeTask(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Task name cannot be empty.");
            }
            _name = name;
            _dependencies = new List<string>();
            _criterias = new List<Func<ICakeContext, bool>>();
            _actions = new List<Action<ICakeContext>>();
        }

        public CakeTask IsDependentOn(string name)
        {
            if (_dependencies.Any(x => x == name))
            {
                const string format = "The task '{0}' already have a dependency on '{1}'.";
                var message = string.Format(format, _name, name);
                throw new CakeException(message);
            }
            _dependencies.Add(name);
            return this;
        }

        public CakeTask WithCriteria(Func<ICakeContext, bool> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }
            _criterias.Add(criteria);
            return this;
        }

        public CakeTask Does(Action<ICakeContext> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            _actions.Add(action);
            return this;
        }
    }
}
