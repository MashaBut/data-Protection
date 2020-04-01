using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PI__3
{
    /// <summary>
    ///     Класс кодирования по ГОСТ 28147-89.
    ///     Для выполнения кодирования надо создать экземпляр класса и добавить ключ (SetKey) и таблицу замен
    /// </summary>
    public class Coder
    {
        #region Fields

        private UInt32[] key;

        private byte[,] replaceTable;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Преобразование из UInt64 в 8 байт
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>3333
        public static byte[] Get8BytesFromUInt64(UInt64 s)
        {
            var result = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                int shift = (56 - 8 * i);
                result[i] = (byte)(s >> shift);
                s = s & (UInt64.MaxValue - ((UInt64)0xff << shift));
            }
            return result;
        }

        /// <summary>
        ///     Преобразование из 8 байт в UInt64
        /// </summary>
        /// <param name="s">массив байт</param>
        /// <param name="startIndex">начальный индекс в массиве</param>
        /// <returns></returns>
        public static UInt64 GetUint64From8Bytes(byte[] s, int startIndex)
        {
            UInt64 result = 0;
            for (int i = startIndex; i < startIndex + 8; i++)
            {
                int shift = (56 - 8 * i);
                result += ((UInt64)s[i] << shift);
            }
            return result;
        }

        /// <summary>
        ///     Формирование имитовставки
        /// </summary>
        /// <param name="plainData">блок открытых данных, размером кратным 8</param>
        /// <returns></returns>
        public byte[] ImitationPaste(byte[] plainData)
        {
            #region проверка

            //if (plainData.Length % 8 != 0)
            //{
            //    throw new ArgumentOutOfRangeException("Размер данных должен быть кратным 8!");
            //}

            if (this.key == null)
            {
                throw new ArgumentNullException("Не задан ключ шифрования!");
            }

            if (this.replaceTable == null)
            {
                throw new ArgumentNullException("Не задана таблица замен!");
            }

            #endregion

            #region преобразование массива байт в массив uint64

            int lenght = plainData.Length / 8;

            var data = new UInt64[lenght];

            for (int i = 0; i < lenght; i++)
            {
                data[i] = GetUint64From8Bytes(plainData, 8 * i);
            }

            #endregion

            #region расшифрование

            ulong crypt = ImitationPaste(data);

            #endregion

            #region преобразование массива uint64 в массив байт

            byte[] result = Get8BytesFromUInt64(crypt);

            #endregion

            return result;
        }

        /// <summary>
        ///     Метод установки ключа - восьми 32-битовых элементов кода
        /// </summary>
        /// <param name="keyData">массив из 32 байт</param>
        public void SetKey(byte[] keyData)
        {
            if (keyData.Length != 32)
            {
                throw new ArgumentOutOfRangeException("Длина массива для ключа должна быть равна 32!");
            }

            this.key = new UInt32[8];
            for (int i = 0; i < keyData.Length; i += 4)
            {
                UInt32 k = keyData[i];
                k = k << 8;
                k = k | keyData[i + 1];
                k = k << 8;
                k = k | keyData[i + 2];
                k = k << 8;
                k = k | keyData[i + 3];
                this.key[i / 4] = k;
            }
        }

        /// <summary>
        ///     Метод установки таблицы замен - матрицы размера 8 х 16, содержащей 4-битовые элементы
        /// </summary>
        /// <param name="replaceTableData">массив из 64 байт</param>
        public void SetReplaceTable(byte[] replaceTableData)
        {
            if (replaceTableData.Length != 64)
            {
                throw new ArgumentOutOfRangeException("Длина массива для ключа должна быть равна 64!");
            }

            this.replaceTable = new byte[8, 16];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int index = i * 8 + j;
                    byte rtd = replaceTableData[index];
                    this.replaceTable[i, j * 2] = (byte)(rtd >> 4);
                    this.replaceTable[i, j * 2 + 1] = (byte)(rtd & 0x0F);
                }
            }
        }

        /// <summary>
        ///     Расшифрование простой заменой
        /// </summary>
        /// <param name="plainData">блок открытых данных, размером кратным 8</param>
        /// <returns></returns>
        public byte[] SimpleDecoding(byte[] plainData)
        {
            #region проверка

            if (plainData.Length % 8 != 0)
            {
                throw new ArgumentOutOfRangeException("Размер данных должен быть кратным 8!");
            }

            if (this.key == null)
            {
                throw new ArgumentNullException("Не задан ключ шифрования!");
            }

            if (this.replaceTable == null)
            {
                throw new ArgumentNullException("Не задана таблица замен!");
            }

            #endregion

            #region преобразование массива байт в массив uint64

            int lenght = plainData.Length / 8;

            var data = new UInt64[lenght];

            for (int i = 0; i < lenght; i++)
            {
                data[i] = GetUint64From8Bytes(plainData, 8 * i);
            }

            #endregion

            #region расшифрование

            ulong[] crypt = SimpleDecoding(data);

            #endregion

            #region преобразование массива uint64 в массив байт

            var result = new byte[plainData.Length];

            for (int i = 0; i < crypt.Length; i++)
            {
                byte[] bytes = Get8BytesFromUInt64(crypt[i]);
                for (int j = 0; j < 8; j++)
                {
                    result[i * 8 + j] = bytes[j];
                }
            }

            #endregion

            return result;
        }

        /// <summary>
        ///     Шифрование простой заменой
        /// </summary>
        /// <param name="plainData">блок открытых данных, размером кратным 8</param>
        /// <returns></returns>
        public byte[] SimpleEncoding(byte[] plainData)
        {
            #region проверка

            if (plainData.Length % 8 != 0)
            {
                throw new ArgumentOutOfRangeException("Размер данных должен быть кратным 8!");
            }

            if (this.key == null)
            {
                throw new ArgumentNullException("Не задан ключ шифрования!");
            }

            if (this.replaceTable == null)
            {
                throw new ArgumentNullException("Не задана таблица замен!");
            }

            #endregion

            #region преобразование массива байт в массив uint64

            int lenght = plainData.Length / 8;

            var data = new UInt64[lenght];

            for (int i = 0; i < lenght; i++)
            {
                data[i] = GetUint64From8Bytes(plainData, 8 * i);
            }

            #endregion

            #region шифрование

            ulong[] crypt = SimpleEncoding(data);

            #endregion

            #region преобразование массива uint64 в массив байт

            var result = new byte[plainData.Length];

            for (int i = 0; i < crypt.Length; i++)
            {
                byte[] bytes = Get8BytesFromUInt64(crypt[i]);
                for (int j = 0; j < 8; j++)
                {
                    result[i * 8 + j] = bytes[j];
                }
            }

            #endregion

            return result;
        }

        #endregion

        #region Methods

        private UInt64 BaseCycle32I(UInt64 data)
        {
            UInt64 result = data;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    result = this.MainCryptoStep(result, this.key[j]);
                }
            }

            return result;
        }

        private UInt64 BaseCycle32R(UInt64 data)
        {
            UInt64 result = data;

            for (int j = 0; j < 8; j++)
            {
                result = this.MainCryptoStep(result, this.key[j]);
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 7; j >= 0; j--)
                {
                    result = this.MainCryptoStep(result, this.key[j]);
                }
            }

            result = ((result & UInt32.MaxValue) << 32) | (result >> 32);

            return result;
        }

        private UInt64 BaseCycle32Z(UInt64 data)
        {
            UInt64 result = data;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    result = this.MainCryptoStep(result, this.key[j]);
                }
            }

            for (int j = 7; j >= 0; j--)
            {
                result = this.MainCryptoStep(result, this.key[j]);
            }

            result = ((result & UInt32.MaxValue) << 32) | (result >> 32);

            return result;
        }

        private uint CycleLeftShift11(uint value)
        {
            uint result = value << 11 | value >> (32 - 11);
            return result;
        }

        private UInt64 ImitationPaste(UInt64[] data)
        {
            UInt64 s = 0;
            s = this.BaseCycle32I(s ^ data[data.Length - 1]);

            return s;
        }

        private UInt64 MainCryptoStep(UInt64 data, UInt32 keyPart)
        {
            #region шаг 0 - разбивание UInt64 на два UInt32

            var n2 = (UInt32)(data >> 32);
            var n1 = (UInt32)(data & UInt32.MaxValue);

            #endregion

            #region шаг 1 - сложение по модулю 2^32

            UInt32 step1Value = n1 + keyPart;

            #endregion

            #region шаг 2 - замена

            uint step2Value = this.ReplaceValues(step1Value);

            #endregion

            #region шаг 3 - сдвиг влево на 11

            uint step3Value = this.CycleLeftShift11(step2Value);

            #endregion

            #region шаг 4 - сложение по модулю 2

            uint step4Value = step3Value ^ n2;

            #endregion

            #region шаг 5 - сдвиг по цепочке

            n2 = n1;
            n1 = step4Value;

            #endregion

            #region шаг 6 - возврат полученного значения

            UInt64 step6Value = (UInt64)n2 << 32 | n1;

            #endregion

            return step6Value;
        }

        private uint ReplaceValues(uint step1Value)
        {
            uint result = 0;
            for (int i = 0; i < 8; i++)
            {
                result <<= 4;
                int shift = 32 - 4 - 4 * i;
                uint index = (step1Value >> shift) & 0xf;
                step1Value = step1Value & (UInt32.MaxValue - ((UInt32)0xf << shift));
                result += this.replaceTable[7 - i, index];
            }
            return result;
        }

        private UInt64[] SimpleDecoding(UInt64[] encryptedData)
        {
            var plainText = new UInt64[encryptedData.Length];
            for (int i = 0; i < encryptedData.Length; i++)
            {
                plainText[i] = this.BaseCycle32R(encryptedData[i]);
            }
            return plainText;
        }

        private UInt64[] SimpleEncoding(UInt64[] plainData)
        {
            var result = new UInt64[plainData.Length];
            for (int i = 0; i < plainData.Length; i++)
            {
                result[i] = this.BaseCycle32Z(plainData[i]);
            }
            return result;
        }
        #endregion
    }
}