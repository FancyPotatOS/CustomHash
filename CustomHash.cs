using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHash
{

    /*
     * 
     * Custom HashMap
     * 
     */

    class CustomHash<T>
    {
        // Print numbers in base 32: 0 - 9, A -> V
        public static readonly string num = "0123456789abcdefghijklmnopqrstuv";

        // Hash Table
        readonly object[] table;

        // Table count
        uint size;

        // Table Size
        readonly uint tableSize;

        public CustomHash(uint ts)
        {
            tableSize = ts;
            size = 0;
            table = new object[ts];
        }

        // Switch to key
        public void Insert(uint id, T data)
        {
            Key<T> key = new Key<T>(id, data);
            Insert(key);
        }

        void Insert(Key<T> key)
        {
            size++;
            uint hash = key.code % tableSize;

            if (table[hash] == null)
            {   
                // Input into space
                table[hash] = key;
            }
            else if (table[hash] is Key<T> preValue)
            {
                // Enforce no duplicate keys
                if (preValue.code == key.code) return;

                // Create a new table in that position
                CustomHash<T> ins = new CustomHash<T>(tableSize);

                // Change the codes
                preValue.code = HashShift(preValue.code);
                key.code = HashShift(key.code);

                ins.Insert(preValue);
                ins.Insert(key);

                table[hash] = ins;
            }
            else if (table[hash] is CustomHash<T> pos)
            {
                key.code = HashShift(key.code);
                pos.Insert(key);
            }
        }
        
        // Returns the position or specified default (or default of T if not)
        public T Get(uint code, T def = default(T))
        {
            uint hash = code % tableSize;

            // Found it!
            if (table[hash] is Key<T>)
            {
                return ((Key<T>) table[hash]).data;
            }

            // Spot is a table!
            else if (table[hash] is CustomHash<T> pos)
            {
                // Get at code after HashShift
                return pos.Get(HashShift(code));
            }

            // If you made it here, it doesn't exist.
            else
            {
                return def;
            }
        }

        // Removes specific object and returns the value or default if it doesn't exist
        public T Remove(uint code)
        {
            uint hash = code % tableSize;

            // Spot is just a key
            if (table[hash] is Key<T> temp)
            {
                table[hash] = null;
                size--;
                return temp.data;
            }

            // Spot is a hash table
            else if (table[hash] is CustomHash<T> pos)
            {
                size--;
                return pos.Remove(HashShift(code));
            }

            // Doesn't exist
            else
            {
                return default(T);
            }
        }

        // Removes specific object if it is equals to the given object
        public bool Remove(uint code, T obj)
        {
            uint hash = code % tableSize;

            if (table[hash] is Key<T> temp && temp.data.Equals(obj))
            {
                table[hash] = null;
                size--;
                return true;
            }
            else if (table[hash] is CustomHash<T> pos)
            {
                size--;
                return pos.Remove(HashShift(code), obj);
            }
            else
            {
                return false;
            }
        }

        // Replaces
        public T Replace(uint code, T obj)
        {
            uint hash = code % tableSize;

            if (table[hash] is Key<T> temp)
            {
                table[hash] = new Key<T>(code, obj);
                return temp.data;
            }
            else if (table[hash] is CustomHash<T> pos)
            {
                return pos.Replace(HashShift(code), obj);
            }
            else
            {
                return default(T);
            }
        }

        public bool Contains(uint code)
        {
            return Contains(code, code);
        }

        public bool Contains(uint code, uint totalCode)
        {
            uint hash = code % tableSize;

            // Spot is a key
            if (table[hash] is Key<T> pos && pos.totalCode == totalCode) { return true; }

            // Spot is a table
            else if (table[hash] is CustomHash<T>) { return ((CustomHash<T>)table[hash]).Contains(HashShift(code), totalCode); }

            // Spot is nothing
            return false;
        }

        // Test if it is empty
        public bool IsEmpty() { return size == 0; }

        // Returns the code divided by table size but as a uinteger rounded down
        // Basically removes the first digit in (base "tableSize")
        // When tableSize = 16, removes the first digit in the number as a hexadecimal number

        // 135 shift 10 = 13
        // 13 shift 10 = 1
        // 17 shift 16 = 16
        // 0xABCD shift 0xD is 0xABC
        public uint HashShift(uint code)
        {
            return (code - (code % tableSize)) / tableSize;
        }

        public string ToPrintString(int layer)
        {
            string space = new string(' ', (int)(layer * 3));
            string coll = "";
            for (int i = 0; i < tableSize; i++)
            {
                if (table[i] is Key<T> key)
                {
                    coll += space + ((tableSize <= num.Length) ? num[i].ToString() : i.ToString()) + ": " + table[i].ToString() + "\n";
                }
                else if (table[i] is CustomHash<T> ch)
                {
                    coll += space + ((tableSize <= num.Length) ? num[i].ToString() : i.ToString()) + ":\n" + ch.ToPrintString(layer + 1) + "\n";
                }
                else
                {
                    coll += space + ((tableSize <= num.Length) ? num[i].ToString() : i.ToString()) + ": " + "null\n";
                }
            }

            return coll;
        }

        public override string ToString()
        {
            string coll = "CustomHash<" + typeof(T).ToString() + ">\n";

            return coll + ToPrintString(0);
        }
    }

    // Key contains the current code, code to get to position, and data
    class Key<T> : IEquatable<T>
    {
        public uint code;
        public uint totalCode;
        public T data;

        public Key(uint c, T d)
        {
            totalCode = c;
            code = c;
            data = d;
        }

        public bool Equals(T obj)
        {
            return (obj.Equals(data));
        }

        public override string ToString()
        {
            return "{" + totalCode + " : " + data.ToString() + "}";
        }
    }
}
