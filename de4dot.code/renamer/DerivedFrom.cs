﻿/*
    Copyright (C) 2011-2012 de4dot@gmail.com

    This file is part of de4dot.

    de4dot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    de4dot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with de4dot.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using de4dot.code.renamer.asmmodules;

namespace de4dot.code.renamer {
	class DerivedFrom {
		Dictionary<string, bool> classNames = new Dictionary<string, bool>(StringComparer.Ordinal);
		Dictionary<TypeDef, bool> results = new Dictionary<TypeDef, bool>();

		public DerivedFrom(string className) {
			addName(className);
		}

		public DerivedFrom(string[] classNames) {
			foreach (var className in classNames)
				addName(className);
		}

		void addName(string className) {
			classNames[className] = true;
		}

		public bool check(TypeDef type) {
			if (results.ContainsKey(type))
				return results[type];

			bool val;
			if (classNames.ContainsKey(type.TypeDefinition.FullName))
				val = true;
			else if (type.baseType == null) {
				if (type.TypeDefinition.BaseType != null)
					val = classNames.ContainsKey(type.TypeDefinition.BaseType.FullName);
				else
					val = false;
			}
			else
				val = check(type.baseType.typeDef);

			results[type] = val;
			return val;
		}
	}
}
