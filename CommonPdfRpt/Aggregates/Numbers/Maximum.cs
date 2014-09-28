﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;

namespace PdfRpt.Aggregates.Numbers
{
    /// <summary>
    /// Maximum function class.
    /// It only works with numbers. If you want to apply it on other data types, you need to create your own AggregateFunction by implementing the IAggregateFunc.
    /// </summary>
    public class Maximum : IAggregateFunction
    {

        /// <summary>
        /// Fires before rendering of this cell.
        /// Now you have time to manipulate the received object and apply your custom formatting function.
        /// It can be null.
        /// </summary>
        public Func<object, string> DisplayFormatFormula { set; get; }

        #region Fields (4)

        double _groupMax;
        long _groupRowNumber;
        double _overallMax;
        long _overallRowNumber;

        #endregion Fields

        #region Properties (2)

        /// <summary>
        /// Returns current groups' aggregate value.
        /// </summary>
        public object GroupValue
        {
            get { return _groupMax; }
        }

        /// <summary>
        /// Returns current row's aggregate value without considering the presence of the groups.
        /// </summary>
        public object OverallValue
        {
            get { return _overallMax; }
        }

        #endregion Properties

        #region Methods (4)

        // Public Methods (1) 

        /// <summary>
        /// Fires after adding a cell to the main table.
        /// </summary>
        /// <param name="cellDataValue">Current cell's data</param>
        /// <param name="isNewGroupStarted">Indicated starting a new group</param>
        public void CellAdded(object cellDataValue, bool isNewGroupStarted)
        {
            checkNewGroupStarted(isNewGroupStarted);

            _groupRowNumber++;
            _overallRowNumber++;

            double cellValue;
            if (double.TryParse(cellDataValue.ToSafeString(), NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out cellValue))
            {
                groupMax(cellValue);
                overallMax(cellValue);
            }
        }
        // Private Methods (3) 

        private void checkNewGroupStarted(bool newGroupStarted)
        {
            if (newGroupStarted)
            {
                _groupMax = 0;
                _groupRowNumber = 0;
            }
        }

        private void groupMax(double cellValue)
        {
            if (_groupRowNumber == 1)
            {
                _groupMax = cellValue;
            }
            else
            {
                if (_groupMax < cellValue)
                    _groupMax = cellValue;
            }
        }

        private void overallMax(double cellValue)
        {
            if (_overallRowNumber == 1)
            {
                _overallMax = cellValue;
            }
            else
            {
                if (_overallMax < cellValue)
                    _overallMax = cellValue;
            }
        }

        /// <summary>
        /// A general method which takes a list of data and calculates its corresponding aggregate value.
        /// It will be used to calculate the aggregate value of each pages individually, with considering the previous pages data.
        /// </summary>
        /// <param name="columnCellsSummaryData">List of data</param>
        /// <returns>Aggregate value</returns>
        public object ProcessingBoundary(IList<SummaryCellData> columnCellsSummaryData)
        {
            if (columnCellsSummaryData == null || !columnCellsSummaryData.Any()) return 0;

            var list = columnCellsSummaryData;

            double max = 0;
            double cellValue;
            if (double.TryParse(list.First().CellData.PropertyValue.ToSafeString(), NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out cellValue))
            {
                max = cellValue;
            }

            foreach (var item in list)
            {
                if (double.TryParse(item.CellData.PropertyValue.ToSafeString(), NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out cellValue))
                {
                    if (cellValue > max)
                        max = cellValue;
                }
            }

            return max;
        }

        #endregion Methods
    }
}
