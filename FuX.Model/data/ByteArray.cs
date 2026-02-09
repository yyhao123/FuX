using FuX.Model.@enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuX.Model.data
{
    public  class ByteArray
    {
        private byte[] m_lBuffer;
        private int m_nReadPosition;
        private int m_nWritePosition;
        private bool m_bCanRead;
        private bool m_bCanWrite;


        private Endianess m_oEndianess;

        public byte[] Buffer
        {
            get { return m_lBuffer; }
        }

        public int Length
        {
            get { return m_lBuffer.Length; }
        }

        public int ReadPosition
        {
            get { return m_nReadPosition; }
            set
            {
                int newPosition = value % Length;
                m_nReadPosition = newPosition;
                m_bCanWrite = true;
            }
        }


        public int WritePosition
        {
            get { return m_nWritePosition; }
            set
            {
                int newPosition = value % Length;
                m_nWritePosition = newPosition;
                m_bCanRead = true;
            }
        }

        public int BytesToRead
        {
            get
            {
                if (!m_bCanRead)
                {
                    return 0;
                }
                if (m_nReadPosition < m_nWritePosition)
                {
                    return m_nWritePosition - m_nReadPosition;
                }
                else if (m_nReadPosition > m_nWritePosition)
                {
                    return Length - m_nReadPosition + m_nWritePosition;
                }
                return 0;
            }
        }

        public int BytesToWrite
        {
            get
            {
                if (!m_bCanWrite)
                {
                    return 0;
                }
                if (m_nReadPosition < m_nWritePosition)
                {
                    return Length - m_nWritePosition + m_nReadPosition;
                }
                else if (m_nReadPosition > m_nWritePosition)
                {
                    return m_nReadPosition - m_nWritePosition;
                }
                return Length;
            }
        }

        public void ResetPosition()
        {
            m_nWritePosition = 0;
            m_nReadPosition = 0;
            m_bCanWrite = true;
            m_bCanRead = false;
        }

        public Endianess endianess
        {
            get { return m_oEndianess; }
            set { m_oEndianess = value; }
        }

        public ByteArray(int size) : this(size, Endianess.BigEndian)
        {
        }

        public ByteArray(int size, Endianess endianess)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            m_lBuffer = new byte[size];
            m_oEndianess = endianess;
            ResetPosition();
        }

        public ByteArray(byte[] byteArray) : this(byteArray, Endianess.BigEndian)
        {
        }

        public ByteArray(byte[] byteArray, Endianess endianess)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException(nameof(byteArray));
            }
            m_lBuffer = new byte[byteArray.Length + 1];
            m_oEndianess = endianess;
            ResetPosition();
            for (var i = 0; i < byteArray.Length; i++)
            {
                Write((byte)byteArray[i]);
            }
        }


        #region 写
        public void Write(bool value)
        {
            if (BytesToWrite < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_lBuffer[WritePosition] = (byte)(value ? 1 : 0);
            WritePosition++;
        }

        public void Write(sbyte value)
        {
            if (BytesToWrite < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_lBuffer[WritePosition] = (byte)value;
            WritePosition++;
        }

        public void Write(byte value)
        {
            if (BytesToWrite < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            m_lBuffer[WritePosition] = value;
            WritePosition++;
        }

        public void Write(short value)
        {
            if (BytesToWrite < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            int idx0, idx1;
            idx0 = WritePosition++;
            idx1 = WritePosition++;
            if (endianess == Endianess.BigEndian)
            {
                m_lBuffer[idx1] = (byte)value;
                m_lBuffer[idx0] = (byte)(value >> 8);
            }
            else
            {
                m_lBuffer[idx0] = (byte)value;
                m_lBuffer[idx1] = (byte)(value >> 8);
            }
        }

        public void Write(ushort value)
        {
            if (BytesToWrite < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            int idx0, idx1;
            idx0 = WritePosition++;
            idx1 = WritePosition++;
            if (endianess == Endianess.BigEndian)
            {
                m_lBuffer[idx1] = (byte)value;
                m_lBuffer[idx0] = (byte)(value >> 8);
            }
            else
            {
                m_lBuffer[idx0] = (byte)value;
                m_lBuffer[idx1] = (byte)(value >> 8);
            }

        }
        #endregion

        #region 读
        public bool ReadBool()
        {
            if (BytesToRead < 1)
            {
                throw new ArgumentOutOfRangeException(" ReadBool ");
            }
            bool result = (m_lBuffer[ReadPosition] != 0);
            ReadPosition++;
            return result;
        }

        public sbyte ReadI8()
        {
            if (BytesToRead < 1)
            {
                throw new ArgumentOutOfRangeException(" ReadI8 ");
            }
            sbyte result = (sbyte)m_lBuffer[ReadPosition];
            ReadPosition++;
            return result;
        }

        public byte ReadU8()
        {
            if (BytesToRead < 1)
            {
                throw new ArgumentOutOfRangeException(" ReadU8 ");
            }
            byte result = m_lBuffer[ReadPosition];
            ReadPosition++;
            return result;
        }


        public short ReadI16()
        {
            if (BytesToRead < 2)
            {
                throw new ArgumentOutOfRangeException(" ReadBool ");
            }
            short result;
            int idx0, idx1;
            idx0 = ReadPosition++;
            idx1 = ReadPosition++;
            if (endianess == Endianess.BigEndian)
            {
                result = (short)(m_lBuffer[idx1] | m_lBuffer[idx0] << 8);
            }
            else
            {
                result = (short)(m_lBuffer[idx0] | m_lBuffer[idx1] << 8);
            }
            return result;
        }

        public ushort ReadU16()
        {
            if (BytesToRead < 2)
            {
                throw new ArgumentOutOfRangeException(" ReadU16 ");
            }
            ushort result;
            int idx0, idx1;
            idx0 = ReadPosition++;
            idx1 = ReadPosition++;
            if (endianess == Endianess.BigEndian)
            {
                result = (ushort)(m_lBuffer[idx1] | m_lBuffer[idx0] << 8);
            }
            else
            {
                result = (ushort)(m_lBuffer[idx0] | m_lBuffer[idx1] << 8);
            }
            return result;
        }

        public void Read(byte[] array, int index, int length)
        {
            if (BytesToRead < length)
            {
                throw new ArgumentOutOfRangeException(" ReadBool ");
            }
            length += index;
            while (index < length)
            {
                array[index] = ReadU8();
                index++;
            }
        }
        #endregion
    }
}
