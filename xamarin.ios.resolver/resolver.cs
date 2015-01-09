// Copyright 2015 Xamarin Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and

using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace Xamarin {

	abstract class Resolver {

		protected abstract string ClassicName { get; }
		protected abstract string ClassicPath { get; }

		protected abstract string UnifiedName { get; }
		protected abstract string UnifiedPath { get; }

		public int Run (string[] args)
		{
			if (args.Length == 0) {
				Console.WriteLine ("Usage: mono {0}.exe assembly", Assembly.GetEntryAssembly ().GetName ().Name);
				return -1;
			}

			string assembly = Path.GetFullPath (args [0]);
			var ar = new DefaultAssemblyResolver ();
			var ad = AssemblyDefinition.ReadAssembly (assembly, new ReaderParameters {
				AssemblyResolver = ar
			});

			// adjust resolver based on classic or unified API
			bool product = false;
			foreach (var aref in ad.MainModule.AssemblyReferences) {
				if (aref.Name == ClassicName) {
					product = true;
					Console.WriteLine ("This assembly refers to {0}.dll (classic) API", ClassicName);
					ar.AddSearchDirectory (ClassicPath);
				} else if (aref.Name == UnifiedName) {
					product = true;
					Console.WriteLine ("This assembly refers to {0}.dll (unified) API", UnifiedName);
					ar.AddSearchDirectory (UnifiedPath);
				}
			}
			if (!product) {
				Console.WriteLine ("'{0}' has no reference to {1}.dll or {2}.dll assemblies", 
					Path.GetFileName (assembly), UnifiedName, ClassicName);
				return -2;
			}

			ar.AddSearchDirectory (Path.GetDirectoryName (assembly));

			int n = 0;
			foreach (var mr in ad.MainModule.GetMemberReferences ()) {
				try {
					// exclude MemberReference that are not methods
					var m = (mr as MethodReference);
					if (m != null)
						m.Resolve ();
				} catch {
					n++;
					Console.WriteLine ("{0} {1} cannot be resolved", mr.DeclaringType, mr);
				}
			}
			Console.WriteLine ("'{0}' resolved with{1} error{2}", 
				Path.GetFileName (assembly), n == 0 ? "out any" : String.Empty, n > 1 ? "s" : String.Empty);
			return n;
		}
	}
}