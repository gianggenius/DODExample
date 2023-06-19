using System;
using System.Collections.Generic;
using System.Reflection;

namespace DODExample
{
    public abstract class BaseTable<TStruct, TRecord>
        where TStruct : ITableData
        where TRecord : ITableRecordData
    {
        public TStruct data;

        protected BaseTable(TStruct data)
        {
            this.data = data;
        }

        public int Length => data.GetType().GetFields()[0].GetValue(data) is Array array ? array.Length : 0;

        /// <summary>
        /// This method can be used to add a Record to Struct of Arrays in Data
        /// </summary>
        /// <param name="record"></param>
        public virtual void AddRecord(TRecord record)
        {
            // Get list of fields in Data and Record
            var fields       = data.GetType().GetFields();
            var recordFields = record.GetType().GetFields();
            
            for (var i = 0; i < fields.Length; i++)
            {
                var value = fields[i].GetValue(data);
                if (value is not Array array) continue;
                
                // Extend the array in Data by 1
                Array resizedArray = Array.CreateInstance(recordFields[i].FieldType, array.Length + 1);
                Array.Copy(array, resizedArray, array.Length);
                
                // Set the last element of the resized Array to the value of the field in Record
                var recordValue = recordFields[i].GetValue(record);
                resizedArray.SetValue(recordValue, array.Length);
                
                // Set the new array to the field in Data
                fields[i].SetValue(data, resizedArray);
            }
        }

        /// <summary>
        /// This method can be used to remove a Record from Struct of Arrays in Data
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Remove success or not</returns>
        public virtual bool RemoveRecord(int index)
        {
            // Get list of fields in Data and Record
            var fields       = data.GetType().GetFields();
            var recordFields = typeof(TRecord).GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var value = fields[i].GetValue(data);
                if (value is not Array array) return false;

                if (index != -1)
                {
                    // Set all the elements after the index to the element before it
                    for (var j = index; j < array.Length - 1; j++)
                    {
                        array.SetValue(array.GetValue(j + 1), j);
                    }

                    // Create a new array with the last element removed
                    Array resizedArray = Array.CreateInstance(recordFields[i].FieldType, array.Length - 1);
                    Array.Copy(array, resizedArray, array.Length - 1);

                    // Set the new array to the field in Data
                    fields[i].SetValue(data, resizedArray);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// This method can be used to update a record in Struct of Arrays in Data by index
        /// </summary>
        /// <param name="record"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual bool UpdateRecordByIndex(TRecord record, int index)
        {
            // Get list of fields in Data and Record
            var fields       = data.GetType().GetFields();
            var recordFields = record.GetType().GetFields();
            for (var i = 0; i < fields.Length; i++)
            {
                var value = fields[i].GetValue(data);
                if (value is not Array array) return false;

                if (index != -1)
                {
                    // Set the value at array index to the value of the field in Record
                    var recordValue = recordFields[i].GetValue(record);
                    array.SetValue(recordValue, index);
                    
                    // Set the new array to the field in Data
                    fields[i].SetValue(data, array);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get a record in Struct of Arrays in Data by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual TRecord GetRecordByIndex(int index)
        {
            var record       = Activator.CreateInstance<TRecord>();
            var dataFields   = data.GetType().GetFields();
            var recordFields = record.GetType().GetFields();

            for (int i = 0; i < dataFields.Length; i++)
            {
                var arrayField  = dataFields[i];
                var resultField = recordFields[i];
                var value       = arrayField.GetValue(data);

                if (value is Array array)
                {
                    if (index >= 0 && index < array.Length)
                    {
                        var element = array.GetValue(index);

                        resultField.SetValue(record, element);
                    }
                }
            }

            return record;
        }
        
    }
}