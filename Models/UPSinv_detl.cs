using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimelyDepotMVC.Models
{
    public class UPSinv_detl
    {
        public global::System.String PROD_CD
        {
            get
            {
                return _PROD_CD;
            }
            set
            {
                if (_PROD_CD != value)
                {
                    _PROD_CD = value;
                }
            }
        }
        private global::System.String _PROD_CD;
        public Nullable<global::System.Decimal> CASE_LEN
        {
            get
            {
                return _CASE_LEN;
            }
            set
            {
                _CASE_LEN = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_LEN;
        public Nullable<global::System.Decimal> CASE_HI
        {
            get
            {
                return _CASE_HI;
            }
            set
            {
                _CASE_HI = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_HI;

        public Nullable<global::System.Decimal> CASE_WI
        {
            get
            {
                return _CASE_WI;
            }
            set
            {
                _CASE_WI = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_WI;

        public Nullable<global::System.Decimal> CASE_WT
        {
            get
            {
                return _CASE_WT;
            }
            set
            {
                _CASE_WT = value;
            }
        }
        private Nullable<global::System.Decimal> _CASE_WT;
        public Nullable<global::System.Decimal> BOX_LEN
        {
            get
            {
                return _BOX_LEN;
            }
            set
            {
                _BOX_LEN = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_LEN;
        public Nullable<global::System.Decimal> BOX_HI
        {
            get
            {
                return _BOX_HI;
            }
            set
            {
                _BOX_HI = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_HI;
        public Nullable<global::System.Decimal> BOX_WI
        {
            get
            {
                return _BOX_WI;
            }
            set
            {
                _BOX_WI = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_WI;
        public Nullable<global::System.Decimal> BOX_WT
        {
            get
            {
                return _BOX_WT;
            }
            set
            {
                _BOX_WT = value;
            }
        }
        private Nullable<global::System.Decimal> _BOX_WT;
        public Nullable<global::System.Decimal> UT_LEN
        {
            get
            {
                return _UT_LEN;
            }
            set
            {
                _UT_LEN = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_LEN;
        public Nullable<global::System.Decimal> UT_HI
        {
            get
            {
                return _UT_HI;
            }
            set
            {
                _UT_HI = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_HI;
        public Nullable<global::System.Decimal> UT_WI
        {
            get
            {
                return _UT_WI;
            }
            set
            {
                _UT_WI = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_WI;
        public Nullable<global::System.Decimal> UT_WT
        {
            get
            {
                return _UT_WT;
            }
            set
            {
                _UT_WT = value;
            }
        }
        private Nullable<global::System.Decimal> _UT_WT;
        public Nullable<global::System.Byte> PD_KG
        {
            get
            {
                return _PD_KG;
            }
            set
            {
                _PD_KG = value;
            }
        }
        private Nullable<global::System.Byte> _PD_KG;
        public global::System.String ORIGEN
        {
            get
            {
                return _ORIGEN;
            }
            set
            {
                _ORIGEN = value;
            }
        }
        private global::System.String _ORIGEN;
    }
}