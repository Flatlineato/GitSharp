﻿/*
 * Copyright (C) 2009, Rolenun <rolenun@gmail.com>
 * Copyrigth (C) 2010, Henon <meinrad.recheis@gmail.com>
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or
 * without modification, are permitted provided that the following
 * conditions are met:
 *
 * - Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * - Redistributions in binary form must reproduce the above
 *   copyright notice, this list of conditions and the following
 *   disclaimer in the documentation and/or other materials provided
 *   with the distribution.
 *
 * - Neither the name of the Git Development Community nor the
 *   names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
 * CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Diagnostics;

namespace GitSharp.Core
{
	public abstract class Platform
	{
		enum GitPlatformID
		{
			Win32S = PlatformID.Win32S,
			Win32Windows = PlatformID.Win32Windows,
			Win32NT = PlatformID.Win32NT,
			WinCE = PlatformID.WinCE,
			Unix = PlatformID.Unix,
			Xbox,
			MacOSX,
		}

		public enum PlatformId
		{
			Windows = 1,
			Linux = 2,
			Mac = 3
		}

		public static Platform GetCurrentPlatform()
		{
			System.OperatingSystem os = Environment.OSVersion;
			GitPlatformID pid = (GitPlatformID)os.Platform;
			Platform obj;

			switch (pid)
			{
				case GitPlatformID.Unix:
					obj = new Linux();
					break;
				case GitPlatformID.MacOSX:
					obj = new Mac();
					break;
				case GitPlatformID.Win32NT:
				case GitPlatformID.Win32S:
				case GitPlatformID.Win32Windows:
				case GitPlatformID.WinCE:
					obj = new Win32();
					break;
				default:
					throw new NotSupportedException("Platform could not be detected!");
			}

			return obj;
		}

		public abstract bool IsHardlinkSupported();


		public abstract bool IsSymlinkSupported();

		public abstract bool CreateSymlink(string symlinkFilename, string existingFilename, bool isSymlinkDirectory);

		public abstract bool CreateHardlink(string hardlinkFilename, string exisitingFilename);


		public abstract Process GetTextPager();

		protected Platform()
		{
		}

		public string ClassName { get; set; }

		public PlatformId Id { get; set; }

		public string PlatformType { get; set; }

		public string PlatformSubType { get; set; }

		public string Edition { get; set; }

		public string Version { get; set; }

		public string VersionFile { get; set; }

		public string ProductName
		{
			get
			{
				return PlatformType + " " + PlatformSubType + " " + Edition + "(" + Version + ")";
			}
		}


	}
}