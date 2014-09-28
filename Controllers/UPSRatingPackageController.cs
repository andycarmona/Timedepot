using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using UPS_Shipping_Rate.UPSRateService;
using TimelyDepotMVC.CommonCode;
using TimelyDepotMVC.Models;
using TimelyDepotMVC.Models.Admin;
using TimelyDepotMVC.DAL;
using System.Net;
using PagedList;
using System.Data;

namespace TimelyDepotMVC.Controllers
{
    using System.Data.Entity;

    public class UPSRatingPackageController : Controller
    {
        //Get the Credentials
        public string szAccessLicenseNumber = "FCBD8E914895FF36";
        public string szUserName = "young55961";
        public string szPassword = "Merced88";
        public string szShipperNumber = "A3024V";
        public string szCustomerCode = "53";

        //Get Shipper Address
        public string szShipperCity = "South El Monte";
        public string szShipperPostalCode = "91733";
        public string szShipperCountryCode = "US";
        public string szShipperStateProvinceCode = "";

        //Get the ship from address
        public string szShipFromCity = "South El Monte";
        public string szShipFromPostalCode = "91733";
        public string szShipFromStateProvinceCode = "CA";
        public string szShipFromCountryCode = "US";

        //Get the ship to address
        public string szShipToCity = "South El Monte";
        //public string szShipToPostalCode = "91733";
        public string szShipToPostalCode = "19850";
        public string szShipToStateProvinceCode = "CA";
        public string szShipToCountryCode = "US";

        private TimelyDepotContext db = new TimelyDepotContext();

        int _pageIndex = 0;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        int _pageSize = 25;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        //
        // GET: /ParametersAdmin/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Parameters parameters = db.Parameters.Find(id);
            if (parameters == null)
            {
                return HttpNotFound();
            }
            return View(parameters);
        }

        //
        // POST: /ParametersAdmin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Parameters parameters)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parameters).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShippingRateTest");
            }
            return View(parameters);
        }

        //
        // GET: //Parameters
        [NoCache]
        public PartialViewResult Parameters()
        {
            string szMsg = "";
            Parameters parameter = null;
            List<Parameters> parametersList = new List<Parameters>();

            //Verify the parameters
            szMsg = "AccessLicenseNumber";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szAccessLicenseNumber;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "UserName";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szUserName;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "Password";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szPassword;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipperNumber";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipperNumber;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "CustomerCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szCustomerCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipperCity";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipperCity;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipperPostalCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipperPostalCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipperCountryCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipperCountryCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipperStateProvinceCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipperStateProvinceCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipFromCity";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipFromCity;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipFromPostalCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipFromPostalCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipFromStateProvinceCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipFromStateProvinceCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipFromCountryCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipFromCountryCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipToCity";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipToCity;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipToPostalCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipToPostalCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipToStateProvinceCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipToStateProvinceCode;
                db.Parameters.Add(parameter);
            }
            parametersList.Add(parameter);
            szMsg = "ShipToCountryCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter == null)
            {
                parameter = new Parameters();
                parameter.Parameter = szMsg;
                parameter.ParameterValue = szShipToCountryCode;
                db.Parameters.Add(parameter);
            }

            db.SaveChanges();

            return PartialView(parametersList);
        }

        //
        // POST: /UPSRatingPackage/GetCost
        //[HttpPost]
        [NoCache]
        public PartialViewResult GetShippingCost(string itemId, string qty, string zipcode, string declarevalue, string negociatednrate)
        {
            int BOX_CASE = 5;
            decimal CASE_WT = 5;
            decimal UT_WT = 1;
            int Qty = 100;
            int diff = 0;

            int nrBoxes = 0;
            int itemsInLastBox = 0;
            int valuePerFullBox = 0;
            int valuePerPartialBox = 0;


            decimal unitPrice = 14;

            string szError = "";
            string szMsg = "";
            string szItemId = "";


            string fullBoxWeight = "";
            string partialBoxWeight = "";

            List<Result> resultList = new List<Result>();

            IQueryable<PRICE> qryPrice = null;

            PackageRateLog rateLog = null;

            inv_detl details = null;

            ITEM item = null;
            PRICE price = null;
            try
            {
                if (!string.IsNullOrEmpty(itemId))
                {
                    szItemId = itemId;
                }

                if (string.IsNullOrEmpty(qty))
                {
                    Qty = 50;
                }
                else
                {
                    Qty = Convert.ToInt32(qty);
                }

                qryPrice = db.PRICEs.Where(prc => prc.Item == szItemId).OrderBy(prc => prc.Qty);
                if (qryPrice.Count() > 0)
                {
                    foreach (var itemPrice in qryPrice)
                    {
                        if (itemPrice.Qty >= Qty)
                        {
                            unitPrice = itemPrice.thePrice;
                            break;
                        }
                    }
                }

                //details = new UPSinv_detl();
                details = new inv_detl();
                details.CASE_HI = 5;
                details.CASE_LEN = 5;
                details.CASE_WI = 5;

                item = db.ITEMs.Where(itm => itm.ItemID == szItemId).FirstOrDefault<ITEM>();
                if (item != null)
                {
                    BOX_CASE = Convert.ToInt16(item.UnitPerCase);
                    CASE_WT = Convert.ToInt32(item.CaseWeight);
                    UT_WT = Convert.ToDecimal(item.UnitWeight);

                    details.CASE_HI = Convert.ToDecimal(item.CaseDimensionH);
                    details.CASE_LEN = Convert.ToDecimal(item.CaseDimensionL);
                    //details.CASE_WT = Convert.ToDecimal(item.CaseDimensionW);   //Sample uses details.CASE_WT
                    details.CASE_WI = Convert.ToDecimal(item.CaseDimensionW);   //Sample uses details.CASE_WT
                    details.CASE_WT = Convert.ToDecimal(item.CaseWeight);
                }

                nrBoxes = Qty / BOX_CASE;
                itemsInLastBox = Qty % BOX_CASE;

                fullBoxWeight = CASE_WT.ToString();
                if (itemsInLastBox > 0)
                {
                    partialBoxWeight = (itemsInLastBox * UT_WT).ToString();
                }

                valuePerFullBox = BOX_CASE * Convert.ToInt32(unitPrice);
                diff = valuePerFullBox % 100;
                if (diff > 0)
                {
                    valuePerFullBox = valuePerFullBox + (100 - diff);
                }

                valuePerPartialBox = itemsInLastBox * Convert.ToInt32(unitPrice);
                diff = valuePerPartialBox % 100;
                if (diff > 0)
                {
                    valuePerPartialBox = valuePerPartialBox + (100 - diff);
                }

                if (!string.IsNullOrEmpty(declarevalue))
                {
                    if (declarevalue == "true")
                    {
                        valuePerFullBox = 0;
                        valuePerPartialBox = 0;
                    }
                }

                //Create the ratelog
                rateLog = new PackageRateLog();
                rateLog.DateSubmit = DateTime.Now;
                rateLog.Boxes = nrBoxes.ToString();
                rateLog.CaseHeight = Convert.ToDecimal(details.CASE_HI).ToString();
                rateLog.CaseLenght = Convert.ToDecimal(details.CASE_LEN).ToString();
                rateLog.CaseWeight = Convert.ToDecimal(details.CASE_WT).ToString();
                rateLog.CaseWidth = Convert.ToDecimal(details.CASE_WI).ToString();
                rateLog.FullBoxWeight = fullBoxWeight;
                rateLog.ItemId = szItemId;
                rateLog.ItemsLastBox = itemsInLastBox.ToString();
                rateLog.PartialBoxWeight = partialBoxWeight;
                rateLog.PostalZipCode = zipcode;
                rateLog.Quantity = Qty.ToString();
                rateLog.UnitPrice = unitPrice.ToString();
                rateLog.ValueperFullBox = valuePerFullBox.ToString();
                rateLog.ValueperPartialBox = valuePerPartialBox.ToString();
                db.PackageRateLogs.Add(rateLog);
                db.SaveChanges();


                //GetRateFromUPS(Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice);
                GetRateFromUPS01(Qty, nrBoxes, BOX_CASE, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice, zipcode, rateLog.Id, declarevalue, negociatednrate, ref resultList);

                if (TempData["ResponseStatus"] != null)
                {
                    ViewBag.ResponseStatus = TempData["ResponseStatus"].ToString();
                }

                if (TempData["AlertMessage"] != null)
                {
                    ViewBag.AlertMessage = TempData["AlertMessage"];
                }

                if (TempData["ErrorRate"] != null)
                {
                    ViewBag.ErrorRate = TempData["ErrorRate"].ToString();
                }

                ViewBag.ItemId = szItemId;
                ViewBag.Quantity = Qty;
                ViewBag.ZipCode = zipcode;
                ViewBag.ResultList = resultList;
                ViewBag.RateLogId = rateLog.Id;
            }
            catch (Exception err)
            {
                szMsg = string.Format("{0} {1}", err.Message, err.StackTrace);
                ViewBag.ErrorMsg = szMsg;
            }
            return PartialView();
        }



        //
        // GetPrice
        public static string GetPrice(TimelyDepotContext db01, string szItem, short nQty)
        {
            string szPrice = "0";
            PRICE price = null;
            IQueryable<PRICE> qryPrice = null;

            //price = db01.PRICEs.Where(prc => prc.Item == szItem && prc.Qty >= nQty).OrderBy(prc => prc.Qty).FirstOrDefault<PRICE>();
            qryPrice = db01.PRICEs.Where(prc => prc.Item == szItem).OrderBy(prc => prc.Qty);
            if (qryPrice.Count() > 0)
            {
                foreach (var item in qryPrice)
                {
                    if (item.Qty >= nQty)
                    {
                        szPrice = item.thePrice.ToString("F2");
                        break;
                    }
                }
            }

            return szPrice;
        }

        //
        // GET: /UPSRatingPackage/SelectItem
        [NoCache]
        public PartialViewResult SelectItem(int? page, string qty, string itemId)
        {
            short nQty = 0;
            int pageIndex = 0;
            int pageSize = PageSize;
            pageSize = 15;


            if (string.IsNullOrEmpty(qty))
            {
                nQty = 50;
            }
            else
            {
                nQty = Convert.ToInt16(qty);
            }
            ViewBag.Qty = nQty;

            IQueryable<ITEM> qryitem = null;

            List<ITEM> itemList = new List<ITEM>();

            qryitem = db.ITEMs.OrderBy(vd => vd.ItemID);

            if (!string.IsNullOrEmpty(itemId))
            {
                qryitem = db.ITEMs.Where(vd => vd.ItemID.StartsWith(itemId)).OrderBy(vd => vd.ItemID);
            }

            if (qryitem.Count() > 0)
            {
                foreach (var item in qryitem)
                {
                    itemList.Add(item);
                }
            }

            //Set the page
            if (page == null)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = Convert.ToInt32(page);
            }


            var onePageOfData = itemList.ToPagedList(pageIndex, pageSize);
            ViewBag.OnePageOfData = onePageOfData;
            return PartialView(itemList.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /UPSRatingPackage/GetShippingRate02
        public ActionResult GetShippingRate02()
        {
            return View();
        }

        //
        // POST: /UPSRatingPackage/GetCost
        [HttpPost]
        public ActionResult GetCost(string qty, string zipcode)
        {
            int BOX_CASE = 5;
            int CASE_WT = 5;
            int UT_WT = 1;
            int Qty = 100;
            int diff = 0;

            int nrBoxes = 0;
            int itemsInLastBox = 0;
            int valuePerFullBox = 0;
            int valuePerPartialBox = 0;


            decimal unitPrice = 14;

            string szError = "";
            string szMsg = "";
            string szItemId = "DF4011";


            string fullBoxWeight = "";
            string partialBoxWeight = "";

            //UPSinv_detl details = null;
            inv_detl details = null;

            ITEM item = null;
            PRICE price = null;
            try
            {
                if (string.IsNullOrEmpty(qty))
                {
                    Qty = 50;
                }
                else
                {
                    Qty = Convert.ToInt32(qty);
                }

                //details = new UPSinv_detl();
                details = new inv_detl();
                details.CASE_HI = 5;
                details.CASE_LEN = 5;
                details.CASE_WI = 5;

                price = db.PRICEs.Where(prc => prc.Item == szItemId && prc.Qty == Qty).FirstOrDefault<PRICE>();
                if (price != null)
                {
                    unitPrice = price.thePrice;
                }

                item = db.ITEMs.Where(itm => itm.ItemID == szItemId).FirstOrDefault<ITEM>();
                if (item != null)
                {
                    BOX_CASE = Convert.ToInt16(item.UnitPerCase);
                    CASE_WT = Convert.ToInt32(item.CaseWeight);
                    UT_WT = Convert.ToInt32(item.UnitWeight);

                    details.CASE_HI = Convert.ToDecimal(item.CaseDimensionH);
                    details.CASE_LEN = Convert.ToDecimal(item.CaseDimensionL);
                    details.CASE_WT = Convert.ToDecimal(item.CaseDimensionW);   //Sample uses details.CASE_WT
                }

                nrBoxes = Qty / BOX_CASE;
                itemsInLastBox = Qty % BOX_CASE;

                fullBoxWeight = CASE_WT.ToString();
                if (itemsInLastBox > 0)
                {
                    partialBoxWeight = (itemsInLastBox * UT_WT).ToString();
                }

                valuePerFullBox = BOX_CASE * Convert.ToInt32(unitPrice);
                diff = valuePerFullBox % 100;
                if (diff > 0)
                {
                    valuePerFullBox = valuePerFullBox + (100 - diff);
                }

                valuePerPartialBox = itemsInLastBox * Convert.ToInt32(unitPrice);
                diff = valuePerPartialBox % 100;
                if (diff > 0)
                {
                    valuePerPartialBox = valuePerPartialBox + (100 - diff);
                }

                //GetRateFromUPS(Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice);
                GetRateFromUPS(Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice);

            }
            catch (Exception err)
            {
                szMsg = string.Format("{0} {1}", err.Message, err.StackTrace);
            }
            return View();
        }

        //
        // GET: /UPSRatingPackage/ShippingRateTest02
        public ActionResult ShippingRateTest02()
        {
            return View();
        }


        //
        // GET: /UPSRatingPackage/ShippingRateTest
        public ActionResult ShippingRateTest()
        {
            // unidades por caja 20, peso por unidad 3.5 Lbs
            int nQty = 100;
            int nrBoxes = 5;
            int nItemsInLastBox = 6;
            int nValuePerFullBox = 160;
            int nValuePerPartialBox = 48;
            decimal dUnitPrice = 8;
            string szCode = "01";
            string szFullBoxWeight = "70";
            string szPartialBoxWeight = "21";
            string szError = "";
            string szMsg = "";
            Parameters parameter = null;
            List<string> errorList = new List<string>();

            UPSResult upresult = new UPSResult();
            UPSinv_detl inv_dtl = new UPSinv_detl();


            //inv_dtl.CASE_HI = 10;
            //inv_dtl.CASE_LEN = 20;
            //inv_dtl.CASE_WI = 12;
            //inv_dtl.CASE_WT = Convert.ToDecimal(3.5);

            //Load Parameters
            LoadParameters();
            szMsg = "ShipToPostalCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                ViewBag.ShipToPostalCode = parameter.ParameterValue;
            }
            else
            {
                ViewBag.ShipToPostalCode = "19850";
            }



            return View();
        }

        private void LoadParameters()
        {
            string szMsg = "";
            Parameters parameter = null;

            //Verify the parameters
            szMsg = "AccessLicenseNumber";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szAccessLicenseNumber = parameter.ParameterValue;
            }
            szMsg = "UserName";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szUserName = parameter.ParameterValue;
            }
            szMsg = "Password";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szPassword = parameter.ParameterValue;
            }
            szMsg = "ShipperNumber";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipperNumber = parameter.ParameterValue;
            }
            szMsg = "CustomerCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szCustomerCode = parameter.ParameterValue;
            }
            szMsg = "ShipperCity";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipperCity = parameter.ParameterValue;
            }
            szMsg = "ShipperPostalCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipperPostalCode = parameter.ParameterValue;
            }
            szMsg = "ShipperCountryCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipperCountryCode = parameter.ParameterValue;
            }
            szMsg = "ShipperStateProvinceCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipperStateProvinceCode = parameter.ParameterValue;
            }
            szMsg = "ShipFromCity";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipFromCity = parameter.ParameterValue;
            }
            szMsg = "ShipFromPostalCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipFromPostalCode = parameter.ParameterValue;
            }
            szMsg = "ShipFromStateProvinceCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipFromStateProvinceCode = parameter.ParameterValue;
            }
            szMsg = "ShipFromCountryCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipFromCountryCode = parameter.ParameterValue;
            }
            szMsg = "ShipToCity";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipToCity = parameter.ParameterValue;
            }
            szMsg = "ShipToPostalCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipToPostalCode = parameter.ParameterValue;
            }
            szMsg = "ShipToStateProvinceCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipToStateProvinceCode = parameter.ParameterValue;
            }
            szMsg = "ShipToCountryCode";
            parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
            if (parameter != null)
            {
                szShipToCountryCode = parameter.ParameterValue;
            }
        }

        //
        // GET: /UPSRatingPackage/MainTest01
        public ActionResult MainTest01()
        {
            string szError = "";
            string szMsg = "";
            string[] requestOption = null;

            List<string> errorList = new List<string>();

            try
            {
                //Use the service and test the behavior
                RateService rate = new RateService();
                RateRequest rateRquest = new RateRequest();
                RateResponse rateResponse = null;
                UPSSecurity upss = new UPSSecurity();
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                RequestType request = new RequestType();
                ShipmentType shipment = new ShipmentType();
                ShipperType shipper = new ShipperType();

                //Set Access License Number
                upssSvcAccessToken.AccessLicenseNumber = szAccessLicenseNumber;
                upss.ServiceAccessToken = upssSvcAccessToken;

                //Set the Credentials
                upssUsrNameToken.Username = szUserName;
                upssUsrNameToken.Password = szPassword;
                upss.UsernameToken = upssUsrNameToken;

                //Set the request option
                requestOption = new string[1];
                requestOption[0] = "Rate";
                request.RequestOption = requestOption;

                //Set the Shipper data
                shipper.ShipperNumber = szShipperNumber;
                AddressType shipperAddress = new AddressType();
                string[] shipperaddressline = { "5555 main", "4 Case Court", "Apt 3B" };
                shipperAddress.AddressLine = shipperaddressline;
                shipperAddress.City = "Roswell";
                shipperAddress.PostalCode = "30076";
                shipperAddress.StateProvinceCode = "GA";
                shipperAddress.CountryCode = "US";
                shipper.Address = shipperAddress;

                //The the ship from address
                ShipFromType shipform = new ShipFromType();
                AddressType shipFromAddress = new AddressType();
                shipFromAddress.AddressLine = shipperaddressline;
                shipFromAddress.City = "Roswell";
                shipFromAddress.PostalCode = "30076";
                shipFromAddress.StateProvinceCode = "GA";
                shipFromAddress.CountryCode = "US";
                shipform.Address = shipFromAddress;
                shipment.ShipFrom = shipform;

                //The ship to address
                ShipToType shipTo = new ShipToType();
                ShipToAddressType shipToAddress = new ShipToAddressType();
                string[] shiptoaddress = { "10 E. Ritchie Way", "2", "Apt 3B" };
                shipToAddress.AddressLine = shiptoaddress;
                shipToAddress.City = "Palm Springs";
                shipToAddress.PostalCode = "92262";
                shipToAddress.StateProvinceCode = "CA";
                shipToAddress.CountryCode = "US";
                shipTo.Address = shipToAddress;
                shipment.ShipTo = shipTo;

                // Set the servide, Below code use  dummy data for reference. Upate as requires
                CodeDescriptionType service = new CodeDescriptionType();
                service.Code = "02";
                shipment.Service = service;

                PackageType package = new PackageType();
                PackageWeightType packageWeight = new PackageWeightType();
                packageWeight.Weight = "125";

                CodeDescriptionType uom = new CodeDescriptionType();
                uom.Code = "LBS";
                uom.Description = "pounds";
                packageWeight.UnitOfMeasurement = uom;
                package.PackageWeight = packageWeight;

                CodeDescriptionType packType = new CodeDescriptionType();
                packType.Code = "02";
                package.PackagingType = packType;

                PackageType[] pkgArray = { package };
                shipment.Package = pkgArray;

                rateRquest.Shipment = shipment;

                // See: http://go.microsoft.com/fwlink?linkid=14202
                // Use ServerCertificateValidationCallback instead
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();

                //Display the results
                ViewBag.RateRequest = rateRquest;

                //Missing data
                //AddCustomerClasification(rateRquest);
                //AddShipper(shipment);
                //rateRquest.Shipment = shipment;

                rateResponse = rate.ProcessRate(rateRquest);

                szMsg = string.Format("Mensaje con uns segunda linea. Hola Vios");
                ViewBag.Message = szMsg;

            }
            catch (System.Web.Services.Protocols.SoapException err)
            {
                szMsg = string.Format("------ Rate Web Service returns error ------");
                errorList.Add(szMsg);
                szMsg = string.Format("------ 'Hard' is user error 'Transient' is system error ------");
                errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException Message={0} ====", err.Message);
                errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException Category:Code:Message={0} ====", err.Detail.LastChild.InnerText);
                errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                errorList.Add(szMsg);
                szMsg = string.Format("SoapException XML Sstring for all={0}", err.Detail.LastChild.OuterXml);
                errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException StackTrace={0} ====", err.StackTrace);
                errorList.Add(szMsg);
                szMsg = string.Format("------------------");
                errorList.Add(szMsg);
                szMsg = string.Format(" ");
                errorList.Add(szMsg);

                ViewBag.ErrorList = errorList;
            }
            catch (Exception err)
            {
                szError = err.Message;
                ViewBag.ErrorMsg = szError;

                szMsg = string.Format("General Exception = {0}", err.Message);
                errorList.Add(szMsg);
                szMsg = string.Format("General Exception-StackTrace = {0}", err.StackTrace);
                errorList.Add(szMsg);

                ViewBag.ErrorList = errorList;
            }
            return View();
        }

        //
        // GET: /UPSRatingPackage/MainTest
        public ActionResult MainTest()
        {
            int nQty = 100;
            int nrBoxes = 5;
            int nItemsInLastBox = 6;
            int nValuePerFullBox = 160;
            int nValuePerPartialBox = 48;
            decimal dUnitPrice = 8.65M;
            string szCode = "01";
            string szFullBoxWeight = "70";
            string szPartialBoxWeight = "21";
            string szError = "";
            string szMsg = "";

            inv_detl details = null;

            List<string> errorList = new List<string>();
            //RateResponse rateResponse = null;

            try
            {
                //Use the service and test the behavior
                RateRequest rateRequest = new RateRequest();
                AddCustomerClassification(rateRequest, szCustomerCode);

                RateService upsService = new RateService();
                AddUpsSecurity(upsService, szAccessLicenseNumber, szUserName, szPassword);

                RequestType request = new RequestType();
                string[] requestOption = { "Rate" };
                request.RequestOption = requestOption;

                ShipmentType shipment = new ShipmentType();

                //Shipper
                string[] shipperaddressline = { "5555 main", "4 Case Court", "Apt 3B" };
                szShipperCity = "Roswell";
                szShipperPostalCode = "30076";
                szShipperStateProvinceCode = "GA";
                szShipperCountryCode = "US";
                AddShipper(shipment, szShipperCity, szShipperPostalCode, szShipperCountryCode, szShipperNumber, shipperaddressline);

                //Ship from address
                string[] shipfromaddressline = { "5555 main", "4 Case Court", "Apt 3B" };
                szShipFromCity = "Roswell";
                szShipFromPostalCode = "30076";
                szShipFromStateProvinceCode = "GA";
                szShipFromCountryCode = "US";
                AddShipFromAddress(shipment, shipfromaddressline, szShipFromCity, szShipFromPostalCode, szShipFromStateProvinceCode, szShipFromCountryCode);

                //Ship to address
                string[] shiptoaddress = { "10 E. Ritchie Way", "2", "Apt 3B" };
                szShipToCity = "Palm Springs";
                szShipToPostalCode = "92262";
                szShipToStateProvinceCode = "CA";
                szShipToCountryCode = "US";
                AddShipToAddress(shipment, shiptoaddress, szShipToCity, szShipToPostalCode, szShipToStateProvinceCode, szShipToCountryCode);

                //Set the service
                UPS_Shipping_Rate.UPSRateService.CodeDescriptionType service = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
                service.Code = "01";
                service.Description = "UPS Next Day Air®";
                shipment.Service = service;
                ShipmentRatingOptionsType optype = new ShipmentRatingOptionsType();
                optype.NegotiatedRatesIndicator = string.Empty;
                shipment.ShipmentRatingOptions = optype;

                //Set the packege parameters
                nrBoxes = 1;
                nItemsInLastBox = 0;
                szFullBoxWeight = "125";
                szPartialBoxWeight = "0";
                nValuePerFullBox = 300;
                nValuePerPartialBox = 0;
                details = new inv_detl();
                details.CASE_HI = 20;
                details.CASE_LEN = 20;
                details.CASE_WI = 5;
                details.CASE_WT = 20;
                AddPackageArray(nrBoxes, nItemsInLastBox, szFullBoxWeight, szPartialBoxWeight, nValuePerFullBox, nValuePerPartialBox, details, shipment);
                AddInvoiceTotalType(nQty, dUnitPrice, shipment);

                rateRequest.Shipment = shipment;
                //  ServicePointManager.ServerCertificateValidationCallback = ValidateRemoteCertificate;
                RateResponse rateResponse = upsService.ProcessRate(rateRequest);


                ViewBag.Message = szMsg;

            }
            catch (System.Web.Services.Protocols.SoapException err)
            {
                szMsg = string.Format("------ Rate Web Service returns error ------");
                errorList.Add(szMsg);
                szMsg = string.Format("------ 'Hard' is user error 'Transient' is system error ------");
                errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException Message={0} ====", err.Message);
                errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException Category:Code:Message={0} ====", err.Detail.LastChild.InnerText);
                errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                errorList.Add(szMsg);
                szMsg = string.Format("SoapException XML Sstring for all={0}", err.Detail.LastChild.OuterXml);
                errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException StackTrace={0} ====", err.StackTrace);
                errorList.Add(szMsg);
                szMsg = string.Format("------------------");
                errorList.Add(szMsg);
                szMsg = string.Format(" ");
                errorList.Add(szMsg);

                ViewBag.ErrorList = errorList;
            }
            catch (Exception err)
            {
                szError = err.Message;
                ViewBag.ErrorMsg = szError;

                szMsg = string.Format("General Exception = {0}", err.Message);
                errorList.Add(szMsg);
                szMsg = string.Format("General Exception-StackTrace = {0}", err.StackTrace);
                errorList.Add(szMsg);

                ViewBag.ErrorList = errorList;
            }
            return View();
        }

        //
        // GET: /UPSRatingPackage/
        public ActionResult Index()
        {
            return View();
        }

        private void AddCustomerClassification(RateRequest rateRequest, string customerCode)
        {
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType customerType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            customerType.Code = customerCode;
            customerType.Description = "";
            rateRequest.CustomerClassification = customerType;
        }

        private void AddCustomerClassification(RateRequest rateRequest)
        {
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType customerType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            customerType.Code = "53";
            customerType.Description = "";
            rateRequest.CustomerClassification = customerType;
        }

        public static void AddUpsSecurity(RateService upsService, string AccessLicenseNumber, string UserName, string Password)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurity upss = new UPS_Shipping_Rate.UPSRateService.UPSSecurity();
            AddUpsServiceAccessToken(upss, AccessLicenseNumber);
            AddUserNameToken(upss, UserName, Password);
            upsService.UPSSecurityValue = upss;
        }
        public static void AddUpsSecurity(RateService upsService)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurity upss = new UPS_Shipping_Rate.UPSRateService.UPSSecurity();
            AddUpsServiceAccessToken(upss);
            AddUserNameToken(upss);
            upsService.UPSSecurityValue = upss;
        }

        private static void AddUpsServiceAccessToken(UPS_Shipping_Rate.UPSRateService.UPSSecurity upss, string AccessLicenseNumber)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurityServiceAccessToken upssSvcAccessToken = new UPS_Shipping_Rate.UPSRateService.UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = AccessLicenseNumber;
            upss.ServiceAccessToken = upssSvcAccessToken;
        }
        private static void AddUpsServiceAccessToken(UPS_Shipping_Rate.UPSRateService.UPSSecurity upss)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurityServiceAccessToken upssSvcAccessToken = new UPS_Shipping_Rate.UPSRateService.UPSSecurityServiceAccessToken();
            upssSvcAccessToken.AccessLicenseNumber = "FCBD8E914895FF36";
            upss.ServiceAccessToken = upssSvcAccessToken;
        }

        private static void AddUserNameToken(UPS_Shipping_Rate.UPSRateService.UPSSecurity upss, string UserName, string Password)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurityUsernameToken upssUsrNameToken = new UPS_Shipping_Rate.UPSRateService.UPSSecurityUsernameToken();
            //upssUsrNameToken.Username = "young55961";
            //upssUsrNameToken.Password = "Merced88";
            upssUsrNameToken.Username = UserName;
            upssUsrNameToken.Password = Password;
            upss.UsernameToken = upssUsrNameToken;
        }
        private static void AddUserNameToken(UPS_Shipping_Rate.UPSRateService.UPSSecurity upss)
        {
            UPS_Shipping_Rate.UPSRateService.UPSSecurityUsernameToken upssUsrNameToken = new UPS_Shipping_Rate.UPSRateService.UPSSecurityUsernameToken();
            //upssUsrNameToken.Username = "young55961";
            //upssUsrNameToken.Password = "Merced88";
            upssUsrNameToken.Username = "young55961";
            upssUsrNameToken.Password = "Merced88";
            upss.UsernameToken = upssUsrNameToken;
        }


        private void AddShipper(ShipmentType shipment, string city, string postalcode, string countrycode, string shippernumber, string[] addressline)
        {
            ShipperType shipper = new ShipperType();
            shipper.ShipperNumber = shippernumber;

            AddShipperAddress(shipper, city, postalcode, countrycode, addressline);

            shipment.Shipper = shipper;
        }
        private void AddShipper(ShipmentType shipment)
        {
            ShipperType shipper = new ShipperType();
            AddShipperAddress(shipper);

            shipment.Shipper = shipper;
        }

        private void AddShipperAddress(ShipperType shipper, string city, string postalcode, string countrycode, string[] addressline)
        {
            AddressType shipperAddress = new AddressType();
            shipperAddress.AddressLine = addressline;
            shipperAddress.City = city;
            shipperAddress.PostalCode = postalcode;
            shipperAddress.CountryCode = countrycode;
            shipper.Address = shipperAddress;
        }
        private void AddShipperAddress(ShipperType shipper)
        {
            AddressType shipperAddress = new AddressType();
            shipperAddress.City = "South El Monte";
            shipperAddress.PostalCode = "91733";
            shipperAddress.CountryCode = "US";
            shipper.ShipperNumber = "A3024V";
            shipper.Address = shipperAddress;
        }

        private static void AddInvoiceTotalType(int Qty, decimal unitPrice, ShipmentType shipment)
        {
            InvoiceLineTotalType invoiceType = new InvoiceLineTotalType();
            invoiceType.CurrencyCode = "USD";
            int total = (int)(Qty * unitPrice);
            if (total % 100 > 0)
                total = total + (100 - total % 100);
            invoiceType.MonetaryValue = total.ToString();
            shipment.InvoiceLineTotal = invoiceType;
        }

        private void AddPackageArray01(string scode, int rateLogId, int nrBoxes, int UnitPerCase, int itemsInLastBox, string fullBoxWeight, string partialBoxWeight, int valuePerFullBox, int valuePerPartialBox, inv_detl details, ShipmentType shipment)
        {
            PackageType[] pkgArray;
            if (itemsInLastBox > 0)
            {
                pkgArray = new PackageType[nrBoxes + 1];
            }
            else
            {
                pkgArray = new PackageType[nrBoxes];
            }

            for (int i = 0; i < nrBoxes; i++)
            {
                AddFullPackage01(scode, rateLogId, UnitPerCase, fullBoxWeight, valuePerFullBox, details, pkgArray, i);
            }
            if (itemsInLastBox > 0 && !string.IsNullOrEmpty(partialBoxWeight))
            {
                AddPartialPackage01(scode, rateLogId, itemsInLastBox, nrBoxes, partialBoxWeight, valuePerPartialBox, details, pkgArray);
            }

            shipment.Package = pkgArray;
        }

        private void AddPackageArray(int nrBoxes, int itemsInLastBox, string fullBoxWeight, string partialBoxWeight, int valuePerFullBox, int valuePerPartialBox, inv_detl details, ShipmentType shipment)
        {
            PackageType[] pkgArray;
            if (itemsInLastBox > 0)
                pkgArray = new PackageType[nrBoxes + 1];
            else
                pkgArray = new PackageType[nrBoxes];

            for (int i = 0; i < nrBoxes; i++)
            {
                AddFullPackage(fullBoxWeight, valuePerFullBox, details, pkgArray, i);
            }
            if (itemsInLastBox > 0 && !string.IsNullOrEmpty(partialBoxWeight))
                AddPartialPackage(nrBoxes, partialBoxWeight, valuePerPartialBox, details, pkgArray);

            shipment.Package = pkgArray;
        }

        private void AddFullPackage01(string scode, int rateLogId, int UnitPerCase, string fullBoxWeight, int valuePerFullBox, inv_detl details, PackageType[] pkgArray, int pos)
        {
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = fullBoxWeight;
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType uom = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            DimensionsType packageDimensions = new DimensionsType();
            packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            packageDimensions.Width = ((int)details.CASE_WI.Value).ToString();
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packDimType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            PackageServiceOptionsType packageServiceOptions = new PackageServiceOptionsType();
            InsuredValueType insuredValue = new InsuredValueType();
            insuredValue.CurrencyCode = "USD";
            insuredValue.MonetaryValue = valuePerFullBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packType.Code = "02";
            package.PackagingType = packType;
            pkgArray[pos] = package;

            if (scode == "03")
            {
                //Create PackageRateLogDetail
                CreatePackageRateLogDetail(scode, rateLogId, pos, UnitPerCase, valuePerFullBox.ToString(), package);
            }
        }

        private void CreatePackageRateLogDetail(string scode, int rateLogId, int nPos, int UnitPerCase, string declaredValue, PackageType package)
        {
            string szMsg = "";
            string szMsgWT = "";

            szMsg = string.Format("{0}x{1}x{2}", package.Dimensions.Width, package.Dimensions.Height, package.Dimensions.Length);
            szMsgWT = string.Format("{0} {1}", package.PackageWeight.Weight, package.PackageWeight.UnitOfMeasurement.Code);

            PackageRateLogDetails ratelogdetail = new PackageRateLogDetails();
            ratelogdetail.IdRateLog = rateLogId;
            ratelogdetail.BoxNo = nPos.ToString();
            ratelogdetail.Quantity = UnitPerCase.ToString();
            ratelogdetail.Dimensions = szMsg;
            ratelogdetail.DimensionsUnits = package.Dimensions.UnitOfMeasurement.Code;
            ratelogdetail.WeigthUnits = szMsgWT;
            ratelogdetail.DeclaredValue = declaredValue;
            ratelogdetail.RequestCode = scode;
            ratelogdetail.PackageTypeCode = package.PackagingType.Code;


            db.PackageRateLogDetails.Add(ratelogdetail);
            db.SaveChanges();
        }

        private void AddFullPackage(string fullBoxWeight, int valuePerFullBox, inv_detl details, PackageType[] pkgArray, int pos)
        {
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = fullBoxWeight;
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType uom = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            DimensionsType packageDimensions = new DimensionsType();
            packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            packageDimensions.Width = ((int)details.CASE_WT.Value).ToString();
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packDimType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            PackageServiceOptionsType packageServiceOptions = new PackageServiceOptionsType();
            InsuredValueType insuredValue = new InsuredValueType();
            insuredValue.CurrencyCode = "USD";
            insuredValue.MonetaryValue = valuePerFullBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packType.Code = "02";
            package.PackagingType = packType;
            pkgArray[pos] = package;
        }
        private void AddPartialPackage01(string scode, int rateLogId, int UnitPerCase, int nrBoxes, string partialBoxWeight, int valuePerPartialBox, inv_detl details, PackageType[] pkgArray)
        {
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = partialBoxWeight;
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType uom = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            DimensionsType packageDimensions = new DimensionsType();
            packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            packageDimensions.Width = ((int)details.CASE_WI.Value).ToString();
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packDimType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            PackageServiceOptionsType packageServiceOptions = new PackageServiceOptionsType();
            InsuredValueType insuredValue = new InsuredValueType();
            insuredValue.CurrencyCode = "USD";
            insuredValue.MonetaryValue = valuePerPartialBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packType.Code = "02";
            package.PackagingType = packType;
            pkgArray[nrBoxes] = package;

            if (scode == "03")
            {
                //Create PackageRateLogDetail
                CreatePackageRateLogDetail(scode, rateLogId, nrBoxes, UnitPerCase, valuePerPartialBox.ToString(), package);
            }

        }

        private void AddPartialPackage(int nrBoxes, string partialBoxWeight, int valuePerPartialBox, inv_detl details, PackageType[] pkgArray)
        {
            PackageType package = new PackageType();
            PackageWeightType packageWeight = new PackageWeightType();
            packageWeight.Weight = partialBoxWeight;
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType uom = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            uom.Code = "LBS";
            uom.Description = "pounds";
            packageWeight.UnitOfMeasurement = uom;
            package.PackageWeight = packageWeight;

            DimensionsType packageDimensions = new DimensionsType();
            packageDimensions.Height = ((int)details.CASE_HI.Value).ToString();
            packageDimensions.Length = ((int)details.CASE_LEN.Value).ToString();
            packageDimensions.Width = ((int)details.CASE_WT.Value).ToString();
            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packDimType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packDimType.Code = "IN";
            packDimType.Description = "Inches";
            packageDimensions.UnitOfMeasurement = packDimType;
            package.Dimensions = packageDimensions;

            PackageServiceOptionsType packageServiceOptions = new PackageServiceOptionsType();
            InsuredValueType insuredValue = new InsuredValueType();
            insuredValue.CurrencyCode = "USD";
            insuredValue.MonetaryValue = valuePerPartialBox.ToString();
            packageServiceOptions.DeclaredValue = insuredValue;
            package.PackageServiceOptions = packageServiceOptions;

            UPS_Shipping_Rate.UPSRateService.CodeDescriptionType packType = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
            packType.Code = "02";
            package.PackagingType = packType;
            pkgArray[nrBoxes] = package;
        }


        private void AddShipToAddress(ShipmentType shipment, string[] addressline, string city, string postalcode, string stateprvincecode, string countrycode)
        {
            ShipToType shipTo = new ShipToType();
            ShipToAddressType shipToAddress = new ShipToAddressType();
            shipToAddress.AddressLine = addressline;
            shipToAddress.City = city;
            shipToAddress.PostalCode = postalcode;
            shipToAddress.StateProvinceCode = stateprvincecode;
            shipToAddress.CountryCode = countrycode;
            shipTo.Address = shipToAddress;
            shipment.ShipTo = shipTo;
        }
        private void AddShipToAddress(ShipmentType shipment)
        {
            ShipToType shipTo = new ShipToType();
            ShipToAddressType shipToAddress = new ShipToAddressType();
            //shipToAddress.PostalCode = txtZipcode.Text;
            shipToAddress.PostalCode = "19850";
            shipToAddress.CountryCode = "US";
            shipTo.Address = shipToAddress;
            shipment.ShipTo = shipTo;
        }

        private void AddShipFromAddress(ShipmentType shipment, string[] addressline, string city, string postalcode, string stateprovincecode, string countrycode)
        {
            ShipFromType shipFrom = new ShipFromType();
            AddressType shipFromAddress = new AddressType();
            shipFromAddress.AddressLine = addressline;
            shipFromAddress.City = city;
            shipFromAddress.PostalCode = postalcode;
            shipFromAddress.StateProvinceCode = stateprovincecode;
            shipFromAddress.CountryCode = countrycode;
            shipFrom.Address = shipFromAddress;
            shipment.ShipFrom = shipFrom;
        }
        private void AddShipFromAddress(ShipmentType shipment)
        {
            ShipFromType shipFrom = new ShipFromType();
            AddressType shipFromAddress = new AddressType();
            //  shipFromAddress.AddressLine = new String[] { txtPShipFromAddress.Text };
            shipFromAddress.City = "South El Monte";
            shipFromAddress.PostalCode = "91733";
            shipFromAddress.StateProvinceCode = "CA";
            shipFromAddress.CountryCode = "US";
            shipFrom.Address = shipFromAddress;
            shipment.ShipFrom = shipFrom;
        }

        private RateResponse CallUPSRateRequest02(string scode, string rservice, int Qty, int nrBoxes, int UnitPerCase, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice, RateService upsService, string zipcode, int rateLogId, string declarevalue, ref string szError)
        {
            string szCustomerCode02 = "";
            string szAccessLicenseNumber02 = "";
            string szUserName02 = "";
            string szPassword02 = "";
            string szRequestOption02 = "Rate";
            string szShipperCity02 = "";
            string szShipperPostalCode02 = "";
            string szShipperCountryCode02 = "";
            string szShipperNumber02 = "";
            string szShipFromCity02 = "";
            string szShipFromPostalCode02 = "";
            string szShipFromStateProvinceCode02 = "";
            string szShipFromCountryCode02 = "";
            string zipcode02 = "";
            string szShipToCountryCode02 = "";
            string szMsg = "";

            Parameters parameter = null;
            RateResponse rateResponse = new RateResponse();
            try
            {
                szMsg = "CustomerCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szCustomerCode02 = parameter.ParameterValue;
                }
                szMsg = "AccessLicenseNumber";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szAccessLicenseNumber02 = parameter.ParameterValue;
                }
                szMsg = "ShipperCountryCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperCountryCode02 = parameter.ParameterValue;
                }
                szMsg = "ShipToCountryCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipToCountryCode02 = parameter.ParameterValue;
                }
                szMsg = "ShipFromCountryCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromCountryCode02 = parameter.ParameterValue;
                }
                szMsg = "ShipperNumber";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperNumber02 = parameter.ParameterValue;
                }
                szMsg = "ShipFromStateProvinceCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromStateProvinceCode02 = parameter.ParameterValue;
                }
                szMsg = "ShipperPostalCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperPostalCode02 = parameter.ParameterValue;
                }
                szMsg = "ShipFromPostalCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromPostalCode02 = parameter.ParameterValue;
                }
                szMsg = "ShipToPostalCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    zipcode02 = parameter.ParameterValue;
                }
                szMsg = "ShipperCity";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperCity02 = parameter.ParameterValue;
                }
                szMsg = "ShipFromCity";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromCity02 = parameter.ParameterValue;
                }
                szMsg = "UserName";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szUserName02 = parameter.ParameterValue;
                }
                szMsg = "Password";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szPassword02 = parameter.ParameterValue;
                }

                RateRequest rateRequest = new RateRequest();
                //AddCustomerClassification(rateRequest);
                AddCustomerClassification(rateRequest, szCustomerCode02);
                ParameterLog(rateLogId, "CustomerCode", szCustomerCode02);

                //AddUpsSecurity(upsService);
                AddUpsSecurity(upsService, szAccessLicenseNumber02, szUserName02, szPassword02);
                ParameterLog(rateLogId, "AccessLicenseNumber", szAccessLicenseNumber02);
                ParameterLog(rateLogId, "UserName", szUserName02);
                ParameterLog(rateLogId, "Password", szPassword02);

                UPS_Shipping_Rate.UPSRateService.RequestType request = new RequestType();
                String[] requestOption = { szRequestOption02 };
                request.RequestOption = requestOption;
                rateRequest.Request = request;
                ParameterLog(rateLogId, "RequestOption", szRequestOption02);

                ShipmentType shipment = new ShipmentType();
                //AddShipper(shipment);
                AddShipper(shipment, szShipperCity02, szShipperPostalCode02, szShipperCountryCode02, szShipperNumber02, null);
                ParameterLog(rateLogId, "ShipperCity", szShipperCity02);
                ParameterLog(rateLogId, "ShipperPostalCode", szShipperPostalCode02);
                ParameterLog(rateLogId, "ShipperCountryCode", szShipperCountryCode02);
                ParameterLog(rateLogId, "ShipperNumber", szShipperNumber02);

                //AddShipFromAddress(shipment);
                AddShipFromAddress(shipment, null, szShipFromCity02, szShipFromPostalCode02, szShipFromStateProvinceCode02, szShipFromCountryCode02);
                ParameterLog(rateLogId, "ShipFromCity", szShipFromCity02);
                ParameterLog(rateLogId, "ShipFromPostalCode", szShipFromPostalCode02);
                ParameterLog(rateLogId, "ShipFromStateProvinceCode", szShipFromStateProvinceCode02);
                ParameterLog(rateLogId, "ShipFromCountryCode", szShipFromCountryCode02);

                //AddShipToAddress(shipment);
                AddShipToAddress(shipment, null, null, zipcode02, null, szShipToCountryCode02);
                ParameterLog(rateLogId, "ShipToPostalCode", zipcode02);
                ParameterLog(rateLogId, "ShipToCountryCode", szShipToCountryCode02);

                UPS_Shipping_Rate.UPSRateService.CodeDescriptionType service = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
                service.Code = scode.ToString();
                shipment.Service = service;
                ParameterLog(rateLogId, "Service Code", scode.ToString());


                ShipmentRatingOptionsType optype = new ShipmentRatingOptionsType();
                optype.NegotiatedRatesIndicator = string.Empty;
                shipment.ShipmentRatingOptions = optype;
                ParameterLog(rateLogId, "NegotiatedRatesIndicator", optype.NegotiatedRatesIndicator);


                AddPackageArray01(scode, rateLogId, nrBoxes, UnitPerCase, itemsInLastBox, fullBoxWeight, partialBoxWeight, valuePerFullBox, valuePerPartialBox, details, shipment);
                if (string.IsNullOrEmpty(declarevalue))
                {
                    AddInvoiceTotalType(Qty, unitPrice, shipment);
                }
                else
                {
                    AddInvoiceTotalType(Qty, 0, shipment);
                }
                rateRequest.Shipment = shipment;
                //  ServicePointManager.ServerCertificateValidationCallback = ValidateRemoteCertificate;
                rateResponse = upsService.ProcessRate(rateRequest);
            }
            catch (System.Web.Services.Protocols.SoapException err)
            {
                szError = string.Format("{0} Code: {1} ({2}) - {3}", szError, scode, rservice, err.Message);

                szMsg = string.Format("------ Rate Web Service returns error ------");
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("------ 'Hard' is user error 'Transient' is system error ------");
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException Message={0} ====", err.Message);
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException Category:Code:Message={0} ====", err.Detail.LastChild.InnerText);
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("SoapException XML Sstring for all={0}", err.Detail.LastChild.OuterXml);
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("=-=-=-=-=-=");
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("==== SoapException StackTrace={0} ====", err.StackTrace);
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                szMsg = string.Format("------------------");
                szError = string.Format("{0} {1}", szError, szMsg);
                //errorList.Add(szMsg);
                //szMsg = string.Format(" ");
                //errorList.Add(szMsg);

            }
            catch (Exception ex)
            {
                //szError = string.Format("{0} Code: {1} - {2} {3}", szError, scode, ex.Message, ex.StackTrace);
                szError = string.Format("{0} Code: {1} ({2}) - {3}", szError, scode, rservice, ex.Message);
            }
            return rateResponse;
        }

        private void ParameterLog(int rateLogId, string szParameter, string szValue)
        {
            PackageRateLogParameters parameterlog = new PackageRateLogParameters();
            parameterlog.IdRateLog = Convert.ToInt32(rateLogId);
            parameterlog.Parameter = szParameter;
            parameterlog.ParameterValue = szValue;
            db.PackageRateLogParameters.Add(parameterlog);
            db.SaveChanges();
        }

        private RateResponse CallUPSRateRequest03(string scode, string rservice, int Qty, int nrBoxes, int UnitPerCase, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice, RateService upsService, string zipcode, int rateLogId, string declarevalue, string negociatednrate, ref string szError)
        {
            string szCustomerCode03 = "";
            string szAccessLicenseNumber03 = "";
            string szUserName03 = "";
            string szPassword03 = "";
            string szRequestOption03 = "Rate";
            string szShipperCity03 = "";
            string szShipperPostalCode03 = "";
            string szShipperCountryCode03 = "";
            string szShipperNumber03 = "";
            string szShipFromCity03 = "";
            string szShipFromPostalCode03 = "";
            string szShipFromStateProvinceCode03 = "";
            string szShipFromCountryCode03 = "";
            string zipcode03 = "";
            string szShipToCountryCode03 = "";
            string szMsg = "";

            Parameters parameter = null;
            RateResponse rateResponse = new RateResponse();
            try
            {
                szMsg = "CustomerCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szCustomerCode03 = parameter.ParameterValue;
                }
                szMsg = "AccessLicenseNumber";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szAccessLicenseNumber03 = parameter.ParameterValue;
                }
                szMsg = "UserName";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szUserName03 = parameter.ParameterValue;
                }
                szMsg = "Password";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szPassword03 = parameter.ParameterValue;
                }
                szMsg = "ShipperCity";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperCity03 = parameter.ParameterValue;
                }
                szMsg = "ShipperPostalCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperPostalCode03 = parameter.ParameterValue;
                }
                szMsg = "ShipperCountryCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperCountryCode03 = parameter.ParameterValue;
                }
                szMsg = "ShipperNumber";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipperNumber03 = parameter.ParameterValue;
                }
                szMsg = "ShipFromCity";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromCity03 = parameter.ParameterValue;
                }
                szMsg = "ShipFromPostalCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromPostalCode03 = parameter.ParameterValue;
                }
                szMsg = "ShipFromStateProvinceCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromStateProvinceCode03 = parameter.ParameterValue;
                }
                szMsg = "ShipFromCountryCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipFromCountryCode03 = parameter.ParameterValue;
                }
                szMsg = "ShipToPostalCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    zipcode03 = parameter.ParameterValue;
                }
                szMsg = "ShipToCountryCode";
                parameter = db.Parameters.Where(pmt => pmt.Parameter == szMsg).FirstOrDefault<Parameters>();
                if (parameter != null)
                {
                    szShipToCountryCode03 = parameter.ParameterValue;
                }


                RateRequest rateRequest = new RateRequest();
                //AddCustomerClassification(rateRequest);
                AddCustomerClassification(rateRequest, szCustomerCode03);
                ParameterLog(rateLogId, "CustomerCode", szCustomerCode03);

                //AddUpsSecurity(upsService);
                AddUpsSecurity(upsService, szAccessLicenseNumber03, szUserName03, szPassword03);
                ParameterLog(rateLogId, "AccessLicenseNumber", szAccessLicenseNumber03);
                ParameterLog(rateLogId, "UserName", szUserName03);
                ParameterLog(rateLogId, "Password", szPassword03);

                UPS_Shipping_Rate.UPSRateService.RequestType request = new RequestType();
                String[] requestOption = { szRequestOption03 };
                request.RequestOption = requestOption;
                rateRequest.Request = request;
                ParameterLog(rateLogId, "RequestOption", szRequestOption03);

                ShipmentType shipment = new ShipmentType();
                //AddShipper(shipment);
                AddShipper(shipment, szShipperCity03, szShipperPostalCode03, szShipperCountryCode03, szShipperNumber03, null);
                ParameterLog(rateLogId, "ShipperCity", szShipperCity03);
                ParameterLog(rateLogId, "ShipperPostalCode", szShipperPostalCode03);
                ParameterLog(rateLogId, "ShipperCountryCode", szShipperCountryCode03);
                ParameterLog(rateLogId, "ShipperNumber", szShipperNumber03);


                //AddShipFromAddress(shipment);
                AddShipFromAddress(shipment, null, szShipFromCity03, szShipFromPostalCode03, szShipFromStateProvinceCode03, szShipFromCountryCode03);
                ParameterLog(rateLogId, "ShipFromCity", szShipFromCity03);
                ParameterLog(rateLogId, "ShipFromPostalCode", szShipFromPostalCode03);
                ParameterLog(rateLogId, "ShipFromStateProvinceCode", szShipFromStateProvinceCode03);
                ParameterLog(rateLogId, "ShipFromCountryCode", szShipFromCountryCode03);

                //AddShipToAddress(shipment);
                AddShipToAddress(shipment, null, null, zipcode03, null, szShipToCountryCode03);
                ParameterLog(rateLogId, "ShipToPostalCode", zipcode03);
                ParameterLog(rateLogId, "ShipToCountryCode", szShipToCountryCode03);


                UPS_Shipping_Rate.UPSRateService.CodeDescriptionType service = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
                service.Code = scode.ToString();
                shipment.Service = service;
                ParameterLog(rateLogId, "Service Code", scode.ToString());

                if (!string.IsNullOrEmpty(negociatednrate))
                {
                    ShipmentRatingOptionsType optype = new ShipmentRatingOptionsType();
                    optype.NegotiatedRatesIndicator = string.Empty;
                    shipment.ShipmentRatingOptions = optype;
                    ParameterLog(rateLogId, "NegotiatedRatesIndicator", optype.NegotiatedRatesIndicator);
                }

                AddPackageArray01(scode, rateLogId, nrBoxes, UnitPerCase, itemsInLastBox, fullBoxWeight, partialBoxWeight, valuePerFullBox, valuePerPartialBox, details, shipment);
                if (string.IsNullOrEmpty(declarevalue))
                {
                    AddInvoiceTotalType(Qty, unitPrice, shipment);
                }
                else
                {
                    AddInvoiceTotalType(Qty, 0, shipment);
                }
                rateRequest.Shipment = shipment;
                //  ServicePointManager.ServerCertificateValidationCallback = ValidateRemoteCertificate;
                rateResponse = upsService.ProcessRate(rateRequest);
            }
            catch (Exception ex)
            {
                //szError = string.Format("{0} Code: {1} - {2} {3}", szError, scode, ex.Message, ex.StackTrace);
                szError = string.Format("{0} Code: {1} ({2}) - {3}", szError, scode, rservice, ex.Message);
            }
            return rateResponse;
        }


        private RateResponse CallUPSRateRequest01(string scode, string rservice, int Qty, int nrBoxes, int UnitPerCase, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice, RateService upsService, string zipcode, int rateLogId, string declarevalue, ref string szError)
        {
            RateResponse rateResponse = new RateResponse();
            try
            {
                RateRequest rateRequest = new RateRequest();
                //AddCustomerClassification(rateRequest);
                AddCustomerClassification(rateRequest, szCustomerCode);
                //AddUpsSecurity(upsService);
                AddUpsSecurity(upsService, szAccessLicenseNumber, szUserName, szPassword);
                UPS_Shipping_Rate.UPSRateService.RequestType request = new RequestType();
                String[] requestOption = { "Rate" };
                request.RequestOption = requestOption;
                rateRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                //AddShipper(shipment);
                AddShipper(shipment, szShipperCity, szShipperPostalCode, szShipperCountryCode, szShipperNumber, null);
                //AddShipFromAddress(shipment);
                AddShipFromAddress(shipment, null, szShipFromCity, szShipFromPostalCode, szShipFromStateProvinceCode, szShipFromCountryCode);
                //AddShipToAddress(shipment);
                AddShipToAddress(shipment, null, null, zipcode, null, szShipToCountryCode);
                UPS_Shipping_Rate.UPSRateService.CodeDescriptionType service = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
                service.Code = scode.ToString();
                shipment.Service = service;
                ShipmentRatingOptionsType optype = new ShipmentRatingOptionsType();
                optype.NegotiatedRatesIndicator = string.Empty;
                shipment.ShipmentRatingOptions = optype;
                AddPackageArray01(scode, rateLogId, nrBoxes, UnitPerCase, itemsInLastBox, fullBoxWeight, partialBoxWeight, valuePerFullBox, valuePerPartialBox, details, shipment);
                if (string.IsNullOrEmpty(declarevalue))
                {
                    AddInvoiceTotalType(Qty, unitPrice, shipment);
                }
                else
                {
                    AddInvoiceTotalType(Qty, 0, shipment);
                }
                rateRequest.Shipment = shipment;
                //  ServicePointManager.ServerCertificateValidationCallback = ValidateRemoteCertificate;
                rateResponse = upsService.ProcessRate(rateRequest);
            }
            catch (Exception ex)
            {
                //szError = string.Format("{0} Code: {1} - {2} {3}", szError, scode, ex.Message, ex.StackTrace);
                szError = string.Format("{0} Code: {1} ({2}) - {3}", szError, scode, rservice, ex.Message);
            }
            return rateResponse;
        }


        private RateResponse CallUPSRateRequest(string scode, int Qty, int nrBoxes, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice, RateService upsService)
        {
            RateResponse rateResponse = new RateResponse();
            try
            {
                RateRequest rateRequest = new RateRequest();
                //AddCustomerClassification(rateRequest);
                AddCustomerClassification(rateRequest, szCustomerCode);
                //AddUpsSecurity(upsService);
                AddUpsSecurity(upsService, szAccessLicenseNumber, szUserName, szPassword);
                UPS_Shipping_Rate.UPSRateService.RequestType request = new RequestType();
                String[] requestOption = { "Rate" };
                request.RequestOption = requestOption;
                rateRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                //AddShipper(shipment);
                AddShipper(shipment, szShipperCity, szShipperPostalCode, szShipperCountryCode, szShipperNumber, null);
                //AddShipFromAddress(shipment);
                AddShipFromAddress(shipment, null, szShipFromCity, szShipFromPostalCode, szShipFromStateProvinceCode, szShipFromCountryCode);
                //AddShipToAddress(shipment);
                AddShipToAddress(shipment, null, null, szShipToPostalCode, null, szShipToCountryCode);
                UPS_Shipping_Rate.UPSRateService.CodeDescriptionType service = new UPS_Shipping_Rate.UPSRateService.CodeDescriptionType();
                service.Code = scode.ToString();
                shipment.Service = service;
                ShipmentRatingOptionsType optype = new ShipmentRatingOptionsType();
                optype.NegotiatedRatesIndicator = string.Empty;
                shipment.ShipmentRatingOptions = optype;
                AddPackageArray(nrBoxes, itemsInLastBox, fullBoxWeight, partialBoxWeight, valuePerFullBox, valuePerPartialBox, details, shipment);
                AddInvoiceTotalType(Qty, unitPrice, shipment);
                rateRequest.Shipment = shipment;
                //  ServicePointManager.ServerCertificateValidationCallback = ValidateRemoteCertificate;
                rateResponse = upsService.ProcessRate(rateRequest);
            }
            catch (Exception ex)
            {

            }
            return rateResponse;
        }

        private void GetRateFromUPS01(int Qty, int nrBoxes, int UnitPerCase, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice, string zipcode, int rateLogId, string declarevalue, string negociatednrate, ref List<Result> lst)
        {
            string szError = "";
            string szError01 = "";
            string szErrorRate = "";
            string szResponseStatus = "";
            string szMsg = "";

            List<string> alertMessagesList = new List<string>();
            CodeDescriptionType1[] alertMessages = null;
            try
            {
                //List<Result> lst = new List<Result>();
                //lst.Add(new Result() { service = "UPS Next Day Air®", code = "01" });
                //lst.Add(new Result() { service = "UPS Second Day Air®", code = "02" });
                lst.Add(new Result() { service = "UPS Ground", code = "03" });

                //lst.Add(new Result() { service = "UPS Express", code = "07" });
                //lst.Add(new Result() { service = "UPS ExpeditedSM", code = "08" });
                //lst.Add(new Result() { service = "UPS Standard", code = "11" });

                //lst.Add(new Result() { service = "UPS Three-Day Select®", code = "12" });
                //lst.Add(new Result() { service = "UPS Next Day Air Saver®", code = "13" });
                //lst.Add(new Result() { service = "UPS Next Day Air® Early A.M. SM", code = "14" });

                foreach (Result r in lst)
                {
                    RateService upsService = new RateService();
                    try
                    {
                        RateResponse rateResponse = CallUPSRateRequest03(r.code, r.service, Qty, nrBoxes, UnitPerCase, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice, upsService, zipcode, rateLogId, declarevalue, negociatednrate, ref szErrorRate);

                        //Get the status and alert messages
                        if (rateResponse.Response != null)
                        {
                            szResponseStatus = rateResponse.Response.ResponseStatus.Description;
                            TempData["ResponseStatus"] = szResponseStatus;

                            alertMessages = rateResponse.Response.Alert;
                            foreach (var item in alertMessages)
                            {
                                szMsg = string.Format("{0} - {1}", item.Code, item.Description);
                                alertMessagesList.Add(szMsg);
                            }
                            TempData["AlertMessage"] = alertMessagesList;
                        }


                        if (rateResponse.RatedShipment != null)
                        {

                            foreach (var rshipment in rateResponse.RatedShipment)
                            {
                                if (rshipment.NegotiatedRateCharges != null)
                                {
                                    r.cost = rshipment.NegotiatedRateCharges.TotalCharge.MonetaryValue + " " + rshipment.NegotiatedRateCharges.TotalCharge.CurrencyCode;
                                }

                                if (rshipment.GuaranteedDelivery != null && rshipment.GuaranteedDelivery.BusinessDaysInTransit != null)
                                {
                                    r.time = rshipment.GuaranteedDelivery.BusinessDaysInTransit + " days";
                                }

                                if (rshipment.BillingWeight != null)
                                {
                                    szMsg = string.Format("{0} {1}", rshipment.BillingWeight.Weight, rshipment.BillingWeight.UnitOfMeasurement.Code);
                                    r.billingweight = szMsg;
                                }
                            }
                        }
                        else
                        {
                            TempData["ErrorRate"] = szErrorRate;
                        }
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {
                        szError01 = string.Format("{0} {1}", ex.Message, ex.StackTrace);
                    }
                }

                //Display the response data
                //ViewBag.ResultList = lst;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                szError = string.Format("{0} {1}", ex.Message, ex.StackTrace);
            }
        }
        private void GetRateFromUPS(int Qty, int nrBoxes, int itemsInLastBox, string fullBoxWeight, int valuePerFullBox, int valuePerPartialBox, string partialBoxWeight, inv_detl details, decimal unitPrice)
        {
            try
            {
                List<Result> lst = new List<Result>();
                lst.Add(new Result() { service = "UPS Next Day Air®", code = "01" });
                lst.Add(new Result() { service = "UPS Second Day Air®", code = "02" });
                lst.Add(new Result() { service = "UPS Ground", code = "03" });
                lst.Add(new Result() { service = "UPS Express", code = "07" });
                lst.Add(new Result() { service = "UPS ExpeditedSM", code = "08" });
                lst.Add(new Result() { service = "UPS Standard", code = "11" });
                lst.Add(new Result() { service = "UPS Three-Day Select®", code = "12" });
                lst.Add(new Result() { service = "UPS Next Day Air Saver®", code = "13" });
                lst.Add(new Result() { service = "UPS Next Day Air® Early A.M. SM", code = "14" });

                foreach (Result r in lst)
                {
                    RateService upsService = new RateService();
                    try
                    {
                        RateResponse rateResponse = CallUPSRateRequest(r.code, Qty, nrBoxes, itemsInLastBox, fullBoxWeight, valuePerFullBox, valuePerPartialBox, partialBoxWeight, details, unitPrice, upsService);
                        if (rateResponse.RatedShipment != null)
                        {
                            foreach (var rshipment in rateResponse.RatedShipment)
                            {
                                r.cost = rshipment.NegotiatedRateCharges.TotalCharge.MonetaryValue + " " + rshipment.NegotiatedRateCharges.TotalCharge.CurrencyCode;
                                if (rshipment.GuaranteedDelivery != null && rshipment.GuaranteedDelivery.BusinessDaysInTransit != null)
                                    r.time = rshipment.GuaranteedDelivery.BusinessDaysInTransit + " days";
                            }
                        }
                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {

                    }
                }
                //lstView.DataSource = lst;
                //lstView.DataBind();

                //Display the response data
                ViewBag.ResultList = lst;
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {

            }
        }
    }
}
