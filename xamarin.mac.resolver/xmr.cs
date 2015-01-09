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

namespace Xamarin {

	class MacResolver : Resolver {

		protected override string ClassicName {
			get { return "XamMac"; }
		}

		protected override string ClassicPath {
			get { return "/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/mono/"; }
		}

		protected override string UnifiedName {
			get { return "Xamarin.Mac"; }
		}

		protected override string UnifiedPath {
			get { return "/Library/Frameworks/Xamarin.Mac.framework/Versions/Current/lib/reference/mobile/"; }
		}

		public static int Main (string[] args)
		{
			return new MacResolver ().Run (args);
		}
	}
}