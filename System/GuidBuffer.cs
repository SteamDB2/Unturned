using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct GuidBuffer
	{
		public GuidBuffer(Guid GUID)
		{
			this = default(GuidBuffer);
			this.GUID = GUID;
		}

		public unsafe void Read(byte[] source, int offset)
		{
			if (offset + 16 > source.Length)
			{
				throw new ArgumentException("Destination buffer is too small!");
			}
			fixed (byte* ptr = (source != null && source.Length != 0) ? ref source[0] : ref *null)
			{
				fixed (ulong* ptr2 = &this.buffer.FixedElementField)
				{
					byte* ptr3 = ptr + (IntPtr)offset;
					ulong* ptr4 = (ulong*)ptr3;
					*ptr2 = *ptr4;
					ptr2[8] = ptr4[1];
				}
			}
		}

		public unsafe void Write(byte[] destination, int offset)
		{
			if (offset + 16 > destination.Length)
			{
				throw new ArgumentException("Destination buffer is too small!");
			}
			fixed (byte* ptr = (destination != null && destination.Length != 0) ? ref destination[0] : ref *null)
			{
				fixed (ulong* ptr2 = &this.buffer.FixedElementField)
				{
					byte* ptr3 = ptr + (IntPtr)offset;
					ulong* ptr4 = (ulong*)ptr3;
					*ptr4 = *ptr2;
					ptr4[1] = ptr2[8];
				}
			}
		}

		public static readonly byte[] GUID_BUFFER = new byte[16];

		[FieldOffset(0)]
		private GuidBuffer.<buffer>__FixedBuffer0 buffer;

		[FieldOffset(0)]
		public Guid GUID;

		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 16)]
		public struct <buffer>__FixedBuffer0
		{
			public ulong FixedElementField;
		}
	}
}
