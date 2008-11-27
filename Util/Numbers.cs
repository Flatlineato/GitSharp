﻿/*
 * Copyright (C) 2008, Shawn O. Pearce <spearce@spearce.org>
 * Copyright (C) 2008, Kevin Thompson <kevin.thompson@theautomaters.com>
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gitty.Extensions;

namespace Gitty.Util
{
    internal class NB
    {
        /**
         * Convert sequence of 4 bytes (network byte order) into unsigned value.
         * 
         * @param intbuf
         *            buffer to acquire the 4 bytes of data from.
         * @param offset
         *            position within the buffer to begin reading from. This
         *            position and the next 3 bytes after it (for a total of 4
         *            bytes) will be read.
         * @return unsigned integer value that matches the 32 bits read.
         */
        public static long DecodeUInt32(byte[] intbuf, int offset)
        {
            long low = (intbuf[offset + 1] & 0xff);
            low <<= 8;

            low |= (byte)(intbuf[offset + 2] & 0xff);
            low <<= 8;

            low |= (byte)(intbuf[offset + 3] & 0xff);

            return ((long)(intbuf[offset] & 0xff) << 24) | low;
        }

        /**
         * Convert sequence of 4 bytes (network byte order) into signed value.
         * 
         * @param intbuf
         *            buffer to acquire the 4 bytes of data from.
         * @param offset
         *            position within the buffer to begin reading from. This
         *            position and the next 3 bytes after it (for a total of 4
         *            bytes) will be read.
         * @return signed integer value that matches the 32 bits read.
         */
        public static int DecodeInt32(byte[] intbuf, int offset)
        {
            int r = intbuf[offset] << 8;

            r |= intbuf[offset + 1] & 0xff;
            r <<= 8;

            r |= intbuf[offset + 2] & 0xff;
            return (r << 8) | (intbuf[offset + 3] & 0xff);
        }

        internal static int CompareUInt32(int a, int b)
        {
            int cmp = a.UnsignedRightShift(1) - b.UnsignedRightShift(1);

            if (cmp != 0)
                return cmp;
            return (a & 1) - (b & 1);
        }



        public static void ReadFully(Stream fd, byte[] dst, int off, int len)
        {
            while (len > 0)
            {
                int r = fd.Read(dst, off, len);
                if (r <= 0)
                    throw new EndOfStreamException("Short read of block.");
                off += r;
                len -= r;
            }
        }

        /**
         * Convert sequence of 8 bytes (network byte order) into unsigned value.
         * 
         * @param intbuf
         *            buffer to acquire the 8 bytes of data from.
         * @param offset
         *            position within the buffer to begin reading from. This
         *            position and the next 7 bytes after it (for a total of 8
         *            bytes) will be read.
         * @return unsigned integer value that matches the 64 bits read.
         */
        public static long DecodeUInt64(byte[] intbuf, int offset)
        {
            return (DecodeUInt32(intbuf, offset) << 32)
                   | DecodeUInt32(intbuf, offset + 4);
        }
    }
}
