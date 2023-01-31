using AspNetCore.Reporting;
using Business.Library;
using Common.Utils;
//using Comone.Utils;
using DataAccess;
using GRBusiness.GoodsReceive;
using GRBusiness.Libs;
using GRBusiness.LPNItem;
using GRDataAccess.Models;
using InboundDataAccess;
using InboundDataAccess.Models;
using MasterDataBusiness.ViewModels;
using Microsoft.EntityFrameworkCore;
using PlanGIBusiness;
using putawayBusiness.ViewModels;
using PutawayDataAccess.Models;
using putawayDataBusiness.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PutawayBusiness.LPN
{
    public class PutawayService
    {
        private PutawayDbContext db;
        private InboundDataAccessDbContext db2;
        private InboundDataAccessDbContext dbInbound; // Fix Update TagItem SavePutAway

        public PutawayService()
        {
            db = new PutawayDbContext();
            db2 = new InboundDataAccessDbContext();
            dbInbound = new InboundDataAccessDbContext();
        }

        public PutawayService(PutawayDbContext db)
        {
            this.db = db;
            this.db2 = db2;

        }

        public List<LPNItemViewModel> scanLpn(LPNItemViewModel data)
        {

            try
            {
                var filterModel = new LPNItemViewModel();
                var result = new List<LPNItemViewModel>();
                var resultErorr = new List<LPNItemViewModel>();


                filterModel.tag_No = data.tag_No;

                //GetConfig
                result = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("TagItemFilter"), filterModel.sJson());

                if (result.Count > 0)
                {
                    var TaskModel = new TaskfilterViewModel();
                    TaskModel.tag_No = data.tag_No;
                    TaskModel.taskGR_No = data.taskGR_No;

                    var resultTask = new TaskfilterViewModel();
                    resultTask = utils.SendDataApi<TaskfilterViewModel>(new AppSettingConfig().GetUrl("CheckTagTask"), TaskModel.sJson());

                    if (resultTask == null)
                    {
                        return resultErorr;

                    }

                }
                else
                {
                    return resultErorr;
                }

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String scanLpnscanPutaway(LPNItemViewModel data)
        {
            try
            {
                string mess = "";
                foreach (var item in data.listLPNItemViewModel)
                {
                    var filterModel = new LPNItemViewModel();
                    var result = new List<LPNItemViewModel>();
                    var resultErorr = new List<LPNItemViewModel>();
                    filterModel.tag_No = item.tag_No;
                    //GetConfig
                    result = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("TagItemFilter"), filterModel.sJson());

                    if (result.Count > 0)
                    {
                        var TaskModel = new TaskfilterViewModel();
                        TaskModel.tag_No = item.tag_No;
                        TaskModel.taskGR_No = item.taskGR_No;

                        var resultTask = new TaskfilterViewModel();
                        resultTask = utils.SendDataApi<TaskfilterViewModel>(new AppSettingConfig().GetUrl("CheckTagTask"), TaskModel.sJson());
                        
                    }
                    var filterModel_location = new LocationConfigViewModel();
                    var result_location = new List<LocationConfigViewModel>();
                    filterModel_location.location_Name = result[0].suggest_Location_Name;

                    //GetConfig
                    result_location = utils.SendDataApi<List<LocationConfigViewModel>>(new AppSettingConfig().GetUrl("LocationConfig"), filterModel_location.sJson());

                    LPNItemViewModel model_save = new LPNItemViewModel();
                    model_save.tag_No = result[0].tag_No;
                    model_save.suggest_Location_Name = result[0].suggest_Location_Name;
                    model_save.goodsReceive_Index = result[0].goodsReceive_Index;
                    model_save.tag_Index = result[0].tag_Index;
                    model_save.confirm_Location_Index = result_location[0].location_Index;
                    model_save.confirm_Location_Id = result_location[0].location_Id;
                    model_save.taskGR_No = item.taskGR_No;

                    mess = SavePutaway(model_save);
                }

                

                return mess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String suggestion(LPNItemViewModel data)
        {

            try
            {

                return "";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LocationConfigViewModel> confirmLocation(LocationConfigViewModel data)
        {

            try
            {
                var filterModel = new LocationConfigViewModel();
                var result = new List<LocationConfigViewModel>();

                filterModel.location_Name = data.location_Name;

                //GetConfig
                result = utils.SendDataApi<List<LocationConfigViewModel>>(new AppSettingConfig().GetUrl("LocationConfig"), filterModel.sJson());

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String SavePutaway(LPNItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            Guid? TaskGRIndex = new Guid();
            Guid? GRIndex = new Guid();


            try
            {
                db2.Database.SetCommandTimeout(90);
                db.Database.SetCommandTimeout(90);
                wm_BinBalance BinBalance = new wm_BinBalance();

                var BinCard = new List<wm_BinCard>();

                wm_TagItem TagItem = new wm_TagItem();

                var filterModel = new LPNItemViewModel();
                var result = new List<LPNItemViewModel>();

                if (data.isSku == false)
                {
                    filterModel.tag_No = data.tag_No;
                }

                else
                {
                    filterModel.tag_No = data.tag_No;
                    filterModel.product_Index = data.product_Index;
                }

                State = "SavePutaway find TagItem";

                result = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("TagItemFilter"), filterModel.sJson());

                foreach (var item in result)
                {

                    var LoModel = new LocationConfigViewModel();
                    var resultLo = new List<LocationConfigViewModel>();

                    LoModel.location_Index = data.confirm_Location_Index;
                    //LoModel.locationType_Index = new Guid("".ToString());
                    resultLo = utils.SendDataApi<List<LocationConfigViewModel>>(new AppSettingConfig().GetUrl("getLocation"), LoModel.sJson());

                    if (resultLo.Count > 0)
                    {

                        if (resultLo.FirstOrDefault().blockPut == 1)
                        {
                            return "blockPut";

                        }
                    }



                    //#region Update StatusTagItem

                    //State = "SavePutaway update TagItem";

                    //var TagItemOld = db2.wm_TagItem.Find(item.tagItem_Index);

                    //TagItemOld.Tag_Status = 2;
                    //TagItemOld.Update_By = data.create_By;
                    //TagItemOld.Update_Date = DateTime.Now;

                    //#endregion

                    #region update BinBalance

                    State = "SavePutaway update BinBalance";

                    var findBinBalance = db.wm_BinBalance.Where(c => c.TagItem_Index == item.tagItem_Index && c.Product_Index == item.product_Index).FirstOrDefault();

                    var BinBalanceOld = db.wm_BinBalance.Find(findBinBalance.BinBalance_Index);

                    BinBalanceOld.Location_Index = data.confirm_Location_Index;
                    BinBalanceOld.Location_Id = data.confirm_Location_Id;
                    BinBalanceOld.Location_Name = data.confirm_Location_Name;
                    BinBalanceOld.Update_By = data.create_By;
                    BinBalanceOld.Update_Date = DateTime.Now;

                    #endregion

                    #region Insert BinCard


                    State = "SavePutaway find GoodsReceiveLocation";

                    var GRLModel = new View_GoodsReceiveViewModel();
                    var resultGRL = new List<View_GoodsReceiveViewModel>();

                    GRLModel.goodsReceiveItem_Index = item.goodsReceiveItem_Index;
                    GRLModel.tagItem_Index = item.tagItem_Index;
                    resultGRL = utils.SendDataApi<List<View_GoodsReceiveViewModel>>(new AppSettingConfig().GetUrl("GRLFilter"), GRLModel.sJson());

                    State = "SavePutaway find TagItem";


                    //var resultTagItem = new List<LPNItemViewModel>();


                    //var listTag = new List<DocumentViewModel> { new DocumentViewModel { documentItem_Index = item.tagItem_Index, ref_documentItem_Index = item.goodsReceiveItem_Index } };
                    //var wTagItem = new DocumentViewModel();
                    //wTagItem.listDocumentViewModel = listTag;

                    //resultTagItem = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("FindTagItem"), wTagItem.sJson());

                    var resultTagItem = db2.wm_TagItem.Where(c => c.TagItem_Index == item.tagItem_Index && c.GoodsReceiveItem_Index == item.goodsReceiveItem_Index).FirstOrDefault();

                    State = "SavePutaway Insert BinCard";

                    GRIndex = resultGRL.FirstOrDefault().goodsReceive_Index;
                    // OUT
                    wm_BinCard BinCardOut = new wm_BinCard();

                    BinCardOut.BinCard_Index = Guid.NewGuid();
                    BinCardOut.BinBalance_Index = BinBalanceOld.BinBalance_Index;
                    BinCardOut.Process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");
                    BinCardOut.DocumentType_Index = resultGRL.FirstOrDefault().documentType_Index;
                    BinCardOut.DocumentType_Id = resultGRL.FirstOrDefault().documentType_Id;
                    BinCardOut.DocumentType_Name = resultGRL.FirstOrDefault().documentType_Name;
                    BinCardOut.GoodsReceive_Index = item.goodsReceive_Index;
                    BinCardOut.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;
                    BinCardOut.GoodsReceiveItemLocation_Index = resultGRL.FirstOrDefault().goodsReceiveItemLocation_Index;
                    BinCardOut.BinCard_No = resultGRL.FirstOrDefault().goodsReceive_No;
                    BinCardOut.BinCard_Date = resultGRL.FirstOrDefault().goodsReceive_Date;

                    BinCardOut.Tag_Index = item.tag_Index;
                    BinCardOut.Tag_Index_To = item.tag_Index;
                    BinCardOut.Tag_No = item.tag_No;
                    BinCardOut.Tag_No_To = item.tag_No;
                    BinCardOut.TagItem_Index = item.tagItem_Index;

                    BinCardOut.Product_Index = resultGRL.FirstOrDefault().product_Index;
                    BinCardOut.Product_Id = resultGRL.FirstOrDefault().product_Id;
                    BinCardOut.Product_Name = resultGRL.FirstOrDefault().product_Name;
                    BinCardOut.Product_SecondName = resultGRL.FirstOrDefault().product_SecondName;
                    BinCardOut.Product_ThirdName = resultGRL.FirstOrDefault().product_Name;
                    BinCardOut.Product_Lot = resultGRL.FirstOrDefault().product_Lot;
                    BinCardOut.Product_Index_To = resultGRL.FirstOrDefault().product_Index;
                    BinCardOut.Product_Id_To = resultGRL.FirstOrDefault().product_Id;
                    BinCardOut.Product_Name_To = resultGRL.FirstOrDefault().product_Name;
                    BinCardOut.Product_SecondName_To = resultGRL.FirstOrDefault().product_SecondName;
                    BinCardOut.Product_ThirdName_To = resultGRL.FirstOrDefault().product_Name;
                    BinCardOut.Product_Lot_To = resultGRL.FirstOrDefault().product_Lot;

                    BinCardOut.ItemStatus_Index = resultGRL.FirstOrDefault().itemStatus_Index;
                    BinCardOut.ItemStatus_Id = resultGRL.FirstOrDefault().itemStatus_Id;
                    BinCardOut.ItemStatus_Name = resultGRL.FirstOrDefault().itemStatus_Name;
                    BinCardOut.ItemStatus_Index_To = resultGRL.FirstOrDefault().itemStatus_Index;
                    BinCardOut.ItemStatus_Id_To = resultGRL.FirstOrDefault().itemStatus_Id;
                    BinCardOut.ItemStatus_Name_To = resultGRL.FirstOrDefault().itemStatus_Name;

                    BinCardOut.ProductConversion_Index = BinBalanceOld.ProductConversion_Index;
                    BinCardOut.ProductConversion_Id = BinBalanceOld.ProductConversion_Id;
                    BinCardOut.ProductConversion_Name = BinBalanceOld.ProductConversion_Name;

                    BinCardOut.Owner_Index = resultGRL.FirstOrDefault().owner_Index;
                    BinCardOut.Owner_Id = resultGRL.FirstOrDefault().owner_Id;
                    BinCardOut.Owner_Name = resultGRL.FirstOrDefault().owner_Name;
                    BinCardOut.Owner_Index_To = resultGRL.FirstOrDefault().owner_Index;
                    BinCardOut.Owner_Id_To = resultGRL.FirstOrDefault().owner_Id;
                    BinCardOut.Owner_Name_To = resultGRL.FirstOrDefault().owner_Name;

                    BinCardOut.Location_Index = (findBinBalance == null) ? resultGRL.FirstOrDefault().location_Index : findBinBalance.Location_Index; //resultGRL.FirstOrDefault().location_Index;
                    BinCardOut.Location_Id = (findBinBalance == null) ? resultGRL.FirstOrDefault().location_Id : findBinBalance.Location_Id; //resultGRL.FirstOrDefault().location_Id;
                    BinCardOut.Location_Name = (findBinBalance == null) ? resultGRL.FirstOrDefault().location_Name : findBinBalance.Location_Name; //resultGRL.FirstOrDefault().location_Name;
                    BinCardOut.Location_Index_To = data.confirm_Location_Index;
                    BinCardOut.Location_Id_To = data.confirm_Location_Id;
                    BinCardOut.Location_Name_To = data.confirm_Location_Name;
                    BinCardOut.GoodsReceive_EXP_Date = resultGRL.FirstOrDefault().eXP_Date;
                    BinCardOut.GoodsReceive_EXP_Date_To = resultGRL.FirstOrDefault().eXP_Date;

                    #region In
                    BinCardOut.BinCard_QtyIn = 0;

                    BinCardOut.BinCard_UnitWeightIn = 0;
                    BinCardOut.BinCard_WeightIn = 0;

                    BinCardOut.BinCard_UnitNetWeightIn = 0;
                    BinCardOut.BinCard_NetWeightIn = 0;

                    BinCardOut.BinCard_UnitGrsWeightIn = 0;
                    BinCardOut.BinCard_GrsWeightIn = 0;

                    BinCardOut.BinCard_UnitWidthIn = 0;
                    BinCardOut.BinCard_WidthIn = 0;

                    BinCardOut.BinCard_UnitLengthIn = 0;
                    BinCardOut.BinCard_LengthIn = 0;

                    BinCardOut.BinCard_UnitHeightIn = 0;
                    BinCardOut.BinCard_HeightIn = 0;

                    BinCardOut.BinCard_UnitVolumeIn = 0;
                    BinCardOut.BinCard_VolumeIn = 0;

                    #endregion

                    #region Out

                    BinCardOut.BinCard_QtyOut = resultTagItem.TotalQty;

                    BinCardOut.BinCard_UnitWeightOut = resultTagItem.UnitWeight;
                    BinCardOut.BinCard_UnitWeightOut_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_UnitWeightOut_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_UnitWeightOut_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_UnitWeightOutRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_WeightOut = resultTagItem.Weight;
                    BinCardOut.BinCard_WeightOut_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_WeightOut_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_WeightOut_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_WeightOutRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_UnitNetWeightOut = resultTagItem.UnitWeight;
                    BinCardOut.BinCard_UnitNetWeightOut_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_UnitNetWeightOut_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_UnitNetWeightOut_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_UnitNetWeightOutRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_NetWeightOut = resultTagItem.NetWeight;
                    BinCardOut.BinCard_NetWeightOut_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_NetWeightOut_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_NetWeightOut_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_NetWeightOutRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_UnitGrsWeightOut = resultTagItem.UnitGrsWeight;
                    BinCardOut.BinCard_UnitGrsWeightOut_Index = resultTagItem.GrsWeight_Index;
                    BinCardOut.BinCard_UnitGrsWeightOut_Id = resultTagItem.GrsWeight_Id;
                    BinCardOut.BinCard_UnitGrsWeightOut_Name = resultTagItem.GrsWeight_Name;
                    BinCardOut.BinCard_UnitGrsWeightOutRatio = resultTagItem.GrsWeightRatio;

                    BinCardOut.BinCard_GrsWeightOut = resultTagItem.GrsWeight;
                    BinCardOut.BinCard_GrsWeightOut_Index = resultTagItem.GrsWeight_Index;
                    BinCardOut.BinCard_GrsWeightOut_Id = resultTagItem.GrsWeight_Id;
                    BinCardOut.BinCard_GrsWeightOut_Name = resultTagItem.GrsWeight_Name;
                    BinCardOut.BinCard_GrsWeightOutRatio = resultTagItem.GrsWeightRatio;

                    BinCardOut.BinCard_UnitWidthOut = resultTagItem.UnitWidth;
                    BinCardOut.BinCard_UnitWidthOut_Index = resultTagItem.Width_Index;
                    BinCardOut.BinCard_UnitWidthOut_Id = resultTagItem.Width_Id;
                    BinCardOut.BinCard_UnitWidthOut_Name = resultTagItem.Width_Name;
                    BinCardOut.BinCard_UnitWidthOutRatio = resultTagItem.WidthRatio;

                    BinCardOut.BinCard_WidthOut = resultTagItem.Width;
                    BinCardOut.BinCard_WidthOut_Index = resultTagItem.Width_Index;
                    BinCardOut.BinCard_WidthOut_Id = resultTagItem.Width_Id;
                    BinCardOut.BinCard_WidthOut_Name = resultTagItem.Width_Name;
                    BinCardOut.BinCard_WidthOutRatio = resultTagItem.WidthRatio;

                    BinCardOut.BinCard_UnitLengthOut = resultTagItem.UnitLength;
                    BinCardOut.BinCard_UnitLengthOut_Index = resultTagItem.Length_Index;
                    BinCardOut.BinCard_UnitLengthOut_Id = resultTagItem.Length_Id;
                    BinCardOut.BinCard_UnitLengthOut_Name = resultTagItem.Length_Name;
                    BinCardOut.BinCard_UnitLengthOutRatio = resultTagItem.LengthRatio;

                    BinCardOut.BinCard_LengthOut = resultTagItem.Length;
                    BinCardOut.BinCard_LengthOut_Index = resultTagItem.Length_Index;
                    BinCardOut.BinCard_LengthOut_Id = resultTagItem.Length_Id;
                    BinCardOut.BinCard_LengthOut_Name = resultTagItem.Length_Name;
                    BinCardOut.BinCard_LengthOutRatio = resultTagItem.LengthRatio;

                    BinCardOut.BinCard_UnitHeightOut = resultTagItem.UnitHeight;
                    BinCardOut.BinCard_UnitHeightOut_Index = resultTagItem.Height_Index;
                    BinCardOut.BinCard_UnitHeightOut_Id = resultTagItem.Height_Id;
                    BinCardOut.BinCard_UnitHeightOut_Name = resultTagItem.Height_Name;
                    BinCardOut.BinCard_UnitHeightOutRatio = resultTagItem.HeightRatio;

                    BinCardOut.BinCard_HeightOut = resultTagItem.Height;
                    BinCardOut.BinCard_HeightOut_Index = resultTagItem.Height_Index;
                    BinCardOut.BinCard_HeightOut_Id = resultTagItem.Height_Id;
                    BinCardOut.BinCard_HeightOut_Name = resultTagItem.Height_Name;
                    BinCardOut.BinCard_HeightOutRatio = resultTagItem.HeightRatio;

                    BinCardOut.BinCard_UnitVolumeOut = resultTagItem.UnitVolume;
                    BinCardOut.BinCard_VolumeOut = resultTagItem.Volume;

                    #endregion

                    #region Sign

                    BinCardOut.BinCard_QtySign = resultTagItem.TotalQty * -1;

                    BinCardOut.BinCard_UnitWeightSign = resultTagItem.UnitWeight *-1;
                    BinCardOut.BinCard_UnitWeightSign_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_UnitWeightSign_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_UnitWeightSign_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_UnitWeightSignRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_WeightSign = resultTagItem.Weight * -1;
                    BinCardOut.BinCard_WeightSign_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_WeightSign_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_WeightSign_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_WeightSignRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_UnitNetWeightSign = resultTagItem.UnitWeight * -1;
                    BinCardOut.BinCard_UnitNetWeightSign_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_UnitNetWeightSign_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_UnitNetWeightSign_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_UnitNetWeightSignRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_NetWeightSign = resultTagItem.NetWeight * -1;
                    BinCardOut.BinCard_NetWeightSign_Index = resultTagItem.Weight_Index;
                    BinCardOut.BinCard_NetWeightSign_Id = resultTagItem.Weight_Id;
                    BinCardOut.BinCard_NetWeightSign_Name = resultTagItem.Weight_Name;
                    BinCardOut.BinCard_NetWeightSignRatio = resultTagItem.WeightRatio;

                    BinCardOut.BinCard_UnitGrsWeightSign = resultTagItem.UnitGrsWeight;
                    BinCardOut.BinCard_UnitGrsWeightSign_Index = resultTagItem.GrsWeight_Index;
                    BinCardOut.BinCard_UnitGrsWeightSign_Id = resultTagItem.GrsWeight_Id;
                    BinCardOut.BinCard_UnitGrsWeightSign_Name = resultTagItem.GrsWeight_Name;
                    BinCardOut.BinCard_UnitGrsWeightSignRatio = resultTagItem.GrsWeightRatio;

                    BinCardOut.BinCard_GrsWeightSign = resultTagItem.GrsWeight * -1;
                    BinCardOut.BinCard_GrsWeightSign_Index = resultTagItem.GrsWeight_Index;
                    BinCardOut.BinCard_GrsWeightSign_Id = resultTagItem.GrsWeight_Id;
                    BinCardOut.BinCard_GrsWeightSign_Name = resultTagItem.GrsWeight_Name;
                    BinCardOut.BinCard_GrsWeightSignRatio = resultTagItem.GrsWeightRatio;

                    BinCardOut.BinCard_UnitWidthSign = resultTagItem.UnitWidth * -1;
                    BinCardOut.BinCard_UnitWidthSign_Index = resultTagItem.Width_Index;
                    BinCardOut.BinCard_UnitWidthSign_Id = resultTagItem.Width_Id;
                    BinCardOut.BinCard_UnitWidthSign_Name = resultTagItem.Width_Name;
                    BinCardOut.BinCard_UnitWidthSignRatio = resultTagItem.WidthRatio;

                    BinCardOut.BinCard_WidthSign = resultTagItem.Width - 1;
                    BinCardOut.BinCard_WidthSign_Index = resultTagItem.Width_Index;
                    BinCardOut.BinCard_WidthSign_Id = resultTagItem.Width_Id;
                    BinCardOut.BinCard_WidthSign_Name = resultTagItem.Width_Name;
                    BinCardOut.BinCard_WidthSignRatio = resultTagItem.WidthRatio;

                    BinCardOut.BinCard_UnitLengthSign = resultTagItem.UnitLength *-1;
                    BinCardOut.BinCard_UnitLengthSign_Index = resultTagItem.Length_Index;
                    BinCardOut.BinCard_UnitLengthSign_Id = resultTagItem.Length_Id;
                    BinCardOut.BinCard_UnitLengthSign_Name = resultTagItem.Length_Name;
                    BinCardOut.BinCard_UnitLengthSignRatio = resultTagItem.LengthRatio;

                    BinCardOut.BinCard_LengthSign = resultTagItem.Length * -1;
                    BinCardOut.BinCard_LengthSign_Index = resultTagItem.Length_Index;
                    BinCardOut.BinCard_LengthSign_Id = resultTagItem.Length_Id;
                    BinCardOut.BinCard_LengthSign_Name = resultTagItem.Length_Name;
                    BinCardOut.BinCard_LengthSignRatio = resultTagItem.LengthRatio;

                    BinCardOut.BinCard_UnitHeightSign = resultTagItem.UnitHeight * -1;
                    BinCardOut.BinCard_UnitHeightSign_Index = resultTagItem.Height_Index;
                    BinCardOut.BinCard_UnitHeightSign_Id = resultTagItem.Height_Id;
                    BinCardOut.BinCard_UnitHeightSign_Name = resultTagItem.Height_Name;
                    BinCardOut.BinCard_UnitHeightSignRatio = resultTagItem.HeightRatio;

                    BinCardOut.BinCard_HeightSign = resultTagItem.Height * -1;
                    BinCardOut.BinCard_HeightSign_Index = resultTagItem.Height_Index;
                    BinCardOut.BinCard_HeightSign_Id = resultTagItem.Height_Id;
                    BinCardOut.BinCard_HeightSign_Name = resultTagItem.Height_Name;
                    BinCardOut.BinCard_HeightSignRatio = resultTagItem.HeightRatio;

                    BinCardOut.BinCard_UnitVolumeSign = resultTagItem.UnitVolume * -1;
                    BinCardOut.BinCard_VolumeSign = resultTagItem.Volume * -1;

                    #endregion

                    BinCardOut.Ref_Document_No = resultGRL.FirstOrDefault().goodsReceive_No;
                    BinCardOut.Ref_Document_Index = resultGRL.FirstOrDefault().goodsReceive_Index;
                    BinCardOut.Ref_DocumentItem_Index = resultGRL.FirstOrDefault().goodsReceiveItem_Index;
                    BinCardOut.Create_By = data.create_By;
                    BinCardOut.Create_Date = DateTime.Now;
                    BinCardOut.BinBalance_Index = BinBalanceOld.BinBalance_Index;
                    BinCardOut.ERP_Location = item.erp_Location;
                    BinCardOut.ERP_Location_To = item.erp_Location;

                    //IN
                    wm_BinCard BinCardIn = new wm_BinCard();

                    BinCardIn.BinCard_Index = Guid.NewGuid();
                    BinCardIn.BinBalance_Index = BinBalanceOld.BinBalance_Index;
                    BinCardIn.Process_Index = new Guid("91FACC8B-A2D2-412B-AF20-03C8971A5867");
                    BinCardIn.DocumentType_Index = resultGRL.FirstOrDefault().documentType_Index;
                    BinCardIn.DocumentType_Id = resultGRL.FirstOrDefault().documentType_Id;
                    BinCardIn.DocumentType_Name = resultGRL.FirstOrDefault().documentType_Name;
                    BinCardIn.GoodsReceive_Index = item.goodsReceive_Index;
                    BinCardIn.GoodsReceiveItem_Index = item.goodsReceiveItem_Index;
                    BinCardIn.GoodsReceiveItemLocation_Index = resultGRL.FirstOrDefault().goodsReceiveItemLocation_Index;
                    BinCardIn.BinCard_No = resultGRL.FirstOrDefault().goodsReceive_No;
                    BinCardIn.BinCard_Date = resultGRL.FirstOrDefault().goodsReceive_Date;

                    BinCardIn.Tag_Index = item.tag_Index;
                    BinCardIn.Tag_Index_To = item.tag_Index;
                    BinCardIn.Tag_No = item.tag_No;
                    BinCardIn.Tag_No_To = item.tag_No;
                    BinCardIn.TagItem_Index = item.tagItem_Index;

                    BinCardIn.Product_Index = resultGRL.FirstOrDefault().product_Index;
                    BinCardIn.Product_Id = resultGRL.FirstOrDefault().product_Id;
                    BinCardIn.Product_Name = resultGRL.FirstOrDefault().product_Name;
                    BinCardIn.Product_SecondName = resultGRL.FirstOrDefault().product_SecondName;
                    BinCardIn.Product_ThirdName = resultGRL.FirstOrDefault().product_Name;
                    BinCardIn.Product_Lot = resultGRL.FirstOrDefault().product_Lot;
                    BinCardIn.Product_Index_To = resultGRL.FirstOrDefault().product_Index;
                    BinCardIn.Product_Id_To = resultGRL.FirstOrDefault().product_Id;
                    BinCardIn.Product_Name_To = resultGRL.FirstOrDefault().product_Name;
                    BinCardIn.Product_SecondName_To = resultGRL.FirstOrDefault().product_SecondName;
                    BinCardIn.Product_ThirdName_To = resultGRL.FirstOrDefault().product_Name;
                    BinCardIn.Product_Lot_To = resultGRL.FirstOrDefault().product_Lot;

                    BinCardIn.ItemStatus_Index = resultGRL.FirstOrDefault().itemStatus_Index;
                    BinCardIn.ItemStatus_Id = resultGRL.FirstOrDefault().itemStatus_Id;
                    BinCardIn.ItemStatus_Name = resultGRL.FirstOrDefault().itemStatus_Name;
                    BinCardIn.ItemStatus_Index_To = resultGRL.FirstOrDefault().itemStatus_Index;
                    BinCardIn.ItemStatus_Id_To = resultGRL.FirstOrDefault().itemStatus_Id;
                    BinCardIn.ItemStatus_Name_To = resultGRL.FirstOrDefault().itemStatus_Name;

                    BinCardIn.ProductConversion_Index = BinBalanceOld.ProductConversion_Index;
                    BinCardIn.ProductConversion_Id = BinBalanceOld.ProductConversion_Id;
                    BinCardIn.ProductConversion_Name = BinBalanceOld.ProductConversion_Name;

                    BinCardIn.Owner_Index = resultGRL.FirstOrDefault().owner_Index;
                    BinCardIn.Owner_Id = resultGRL.FirstOrDefault().owner_Id;
                    BinCardIn.Owner_Name = resultGRL.FirstOrDefault().owner_Name;
                    BinCardIn.Owner_Index_To = resultGRL.FirstOrDefault().owner_Index;
                    BinCardIn.Owner_Id_To = resultGRL.FirstOrDefault().owner_Id;
                    BinCardIn.Owner_Name_To = resultGRL.FirstOrDefault().owner_Name;

                    BinCardIn.Location_Index = data.confirm_Location_Index;
                    BinCardIn.Location_Id = data.confirm_Location_Id;
                    BinCardIn.Location_Name = data.confirm_Location_Name;
                    BinCardIn.Location_Index_To = data.confirm_Location_Index;
                    BinCardIn.Location_Id_To = data.confirm_Location_Id;
                    BinCardIn.Location_Name_To = data.confirm_Location_Name;

                    BinCardIn.GoodsReceive_EXP_Date = resultGRL.FirstOrDefault().eXP_Date;
                    BinCardIn.GoodsReceive_EXP_Date_To = resultGRL.FirstOrDefault().eXP_Date;

                    #region In

                    BinCardIn.BinCard_QtyIn = resultTagItem.TotalQty;

                    BinCardIn.BinCard_UnitWeightIn = resultTagItem.UnitWeight;
                    BinCardIn.BinCard_UnitWeightIn_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_UnitWeightIn_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_UnitWeightIn_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_UnitWeightInRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_WeightIn = resultTagItem.Weight;
                    BinCardIn.BinCard_WeightIn_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_WeightIn_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_WeightIn_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_WeightInRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_UnitNetWeightIn = resultTagItem.UnitWeight;
                    BinCardIn.BinCard_UnitNetWeightIn_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_UnitNetWeightIn_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_UnitNetWeightIn_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_UnitNetWeightInRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_NetWeightIn = resultTagItem.NetWeight;
                    BinCardIn.BinCard_NetWeightIn_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_NetWeightIn_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_NetWeightIn_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_NetWeightInRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_UnitGrsWeightIn = resultTagItem.UnitGrsWeight;
                    BinCardIn.BinCard_UnitGrsWeightIn_Index = resultTagItem.GrsWeight_Index;
                    BinCardIn.BinCard_UnitGrsWeightIn_Id = resultTagItem.GrsWeight_Id;
                    BinCardIn.BinCard_UnitGrsWeightIn_Name = resultTagItem.GrsWeight_Name;
                    BinCardIn.BinCard_UnitGrsWeightInRatio = resultTagItem.GrsWeightRatio;

                    BinCardIn.BinCard_GrsWeightIn = resultTagItem.GrsWeight;
                    BinCardIn.BinCard_GrsWeightIn_Index = resultTagItem.GrsWeight_Index;
                    BinCardIn.BinCard_GrsWeightIn_Id = resultTagItem.GrsWeight_Id;
                    BinCardIn.BinCard_GrsWeightIn_Name = resultTagItem.GrsWeight_Name;
                    BinCardIn.BinCard_GrsWeightInRatio = resultTagItem.GrsWeightRatio;

                    BinCardIn.BinCard_UnitWidthIn = resultTagItem.UnitWidth;
                    BinCardIn.BinCard_UnitWidthIn_Index = resultTagItem.Width_Index;
                    BinCardIn.BinCard_UnitWidthIn_Id = resultTagItem.Width_Id;
                    BinCardIn.BinCard_UnitWidthIn_Name = resultTagItem.Width_Name;
                    BinCardIn.BinCard_UnitWidthInRatio = resultTagItem.WidthRatio;

                    BinCardIn.BinCard_WidthIn = resultTagItem.Width;
                    BinCardIn.BinCard_WidthIn_Index = resultTagItem.Width_Index;
                    BinCardIn.BinCard_WidthIn_Id = resultTagItem.Width_Id;
                    BinCardIn.BinCard_WidthIn_Name = resultTagItem.Width_Name;
                    BinCardIn.BinCard_WidthInRatio = resultTagItem.WidthRatio;

                    BinCardIn.BinCard_UnitLengthIn = resultTagItem.UnitLength;
                    BinCardIn.BinCard_UnitLengthIn_Index = resultTagItem.Length_Index;
                    BinCardIn.BinCard_UnitLengthIn_Id = resultTagItem.Length_Id;
                    BinCardIn.BinCard_UnitLengthIn_Name = resultTagItem.Length_Name;
                    BinCardIn.BinCard_UnitLengthInRatio = resultTagItem.LengthRatio;

                    BinCardIn.BinCard_LengthIn = resultTagItem.Length;
                    BinCardIn.BinCard_LengthIn_Index = resultTagItem.Length_Index;
                    BinCardIn.BinCard_LengthIn_Id = resultTagItem.Length_Id;
                    BinCardIn.BinCard_LengthIn_Name = resultTagItem.Length_Name;
                    BinCardIn.BinCard_LengthInRatio = resultTagItem.LengthRatio;

                    BinCardIn.BinCard_UnitHeightIn = resultTagItem.UnitHeight;
                    BinCardIn.BinCard_UnitHeightIn_Index = resultTagItem.Height_Index;
                    BinCardIn.BinCard_UnitHeightIn_Id = resultTagItem.Height_Id;
                    BinCardIn.BinCard_UnitHeightIn_Name = resultTagItem.Height_Name;
                    BinCardIn.BinCard_UnitHeightInRatio = resultTagItem.HeightRatio;

                    BinCardIn.BinCard_HeightIn = resultTagItem.Height;
                    BinCardIn.BinCard_HeightIn_Index = resultTagItem.Height_Index;
                    BinCardIn.BinCard_HeightIn_Id = resultTagItem.Height_Id;
                    BinCardIn.BinCard_HeightIn_Name = resultTagItem.Height_Name;
                    BinCardIn.BinCard_HeightInRatio = resultTagItem.HeightRatio;

                    BinCardIn.BinCard_UnitVolumeIn = resultTagItem.UnitVolume;
                    BinCardIn.BinCard_VolumeIn = resultTagItem.Volume;

                    #endregion

                    #region Out

                    BinCardIn.BinCard_QtyOut = 0;

                    BinCardIn.BinCard_UnitWeightOut = 0;
                    BinCardIn.BinCard_WeightOut = 0;

                    BinCardIn.BinCard_UnitNetWeightOut = 0;
                    BinCardIn.BinCard_NetWeightOut = 0;

                    BinCardIn.BinCard_UnitGrsWeightOut = 0;
                    BinCardIn.BinCard_GrsWeightOut = 0;

                    BinCardIn.BinCard_UnitWidthOut = 0;
                    BinCardIn.BinCard_WidthOut = 0;

                    BinCardIn.BinCard_UnitLengthOut = 0;
                    BinCardIn.BinCard_LengthOut = 0;

                    BinCardIn.BinCard_UnitHeightOut = 0;
                    BinCardIn.BinCard_HeightOut = 0;

                    #endregion

                    #region Sign

                    BinCardIn.BinCard_QtySign = resultTagItem.TotalQty;

                    BinCardIn.BinCard_UnitWeightSign = resultTagItem.UnitWeight;
                    BinCardIn.BinCard_UnitWeightSign_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_UnitWeightSign_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_UnitWeightSign_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_UnitWeightSignRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_WeightSign = resultTagItem.Weight;
                    BinCardIn.BinCard_WeightSign_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_WeightSign_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_WeightSign_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_WeightSignRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_UnitNetWeightSign = resultTagItem.UnitWeight;
                    BinCardIn.BinCard_UnitNetWeightSign_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_UnitNetWeightSign_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_UnitNetWeightSign_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_UnitNetWeightSignRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_NetWeightSign = resultTagItem.NetWeight;
                    BinCardIn.BinCard_NetWeightSign_Index = resultTagItem.Weight_Index;
                    BinCardIn.BinCard_NetWeightSign_Id = resultTagItem.Weight_Id;
                    BinCardIn.BinCard_NetWeightSign_Name = resultTagItem.Weight_Name;
                    BinCardIn.BinCard_NetWeightSignRatio = resultTagItem.WeightRatio;

                    BinCardIn.BinCard_UnitGrsWeightSign = resultTagItem.UnitGrsWeight;
                    BinCardIn.BinCard_UnitGrsWeightSign_Index = resultTagItem.GrsWeight_Index;
                    BinCardIn.BinCard_UnitGrsWeightSign_Id = resultTagItem.GrsWeight_Id;
                    BinCardIn.BinCard_UnitGrsWeightSign_Name = resultTagItem.GrsWeight_Name;
                    BinCardIn.BinCard_UnitGrsWeightSignRatio = resultTagItem.GrsWeightRatio;

                    BinCardIn.BinCard_GrsWeightSign = resultTagItem.GrsWeight;
                    BinCardIn.BinCard_GrsWeightSign_Index = resultTagItem.GrsWeight_Index;
                    BinCardIn.BinCard_GrsWeightSign_Id = resultTagItem.GrsWeight_Id;
                    BinCardIn.BinCard_GrsWeightSign_Name = resultTagItem.GrsWeight_Name;
                    BinCardIn.BinCard_GrsWeightSignRatio = resultTagItem.GrsWeightRatio;

                    BinCardIn.BinCard_UnitWidthSign = resultTagItem.UnitWidth;
                    BinCardIn.BinCard_UnitWidthSign_Index = resultTagItem.Width_Index;
                    BinCardIn.BinCard_UnitWidthSign_Id = resultTagItem.Width_Id;
                    BinCardIn.BinCard_UnitWidthSign_Name = resultTagItem.Width_Name;
                    BinCardIn.BinCard_UnitWidthSignRatio = resultTagItem.WidthRatio;

                    BinCardIn.BinCard_WidthSign = resultTagItem.Width;
                    BinCardIn.BinCard_WidthSign_Index = resultTagItem.Width_Index;
                    BinCardIn.BinCard_WidthSign_Id = resultTagItem.Width_Id;
                    BinCardIn.BinCard_WidthSign_Name = resultTagItem.Width_Name;
                    BinCardIn.BinCard_WidthSignRatio = resultTagItem.WidthRatio;

                    BinCardIn.BinCard_UnitLengthSign = resultTagItem.UnitLength;
                    BinCardIn.BinCard_UnitLengthSign_Index = resultTagItem.Length_Index;
                    BinCardIn.BinCard_UnitLengthSign_Id = resultTagItem.Length_Id;
                    BinCardIn.BinCard_UnitLengthSign_Name = resultTagItem.Length_Name;
                    BinCardIn.BinCard_UnitLengthSignRatio = resultTagItem.LengthRatio;

                    BinCardIn.BinCard_LengthSign = resultTagItem.Length;
                    BinCardIn.BinCard_LengthSign_Index = resultTagItem.Length_Index;
                    BinCardIn.BinCard_LengthSign_Id = resultTagItem.Length_Id;
                    BinCardIn.BinCard_LengthSign_Name = resultTagItem.Length_Name;
                    BinCardIn.BinCard_LengthSignRatio = resultTagItem.LengthRatio;

                    BinCardIn.BinCard_UnitHeightSign = resultTagItem.UnitHeight;
                    BinCardIn.BinCard_UnitHeightSign_Index = resultTagItem.Height_Index;
                    BinCardIn.BinCard_UnitHeightSign_Id = resultTagItem.Height_Id;
                    BinCardIn.BinCard_UnitHeightSign_Name = resultTagItem.Height_Name;
                    BinCardIn.BinCard_UnitHeightSignRatio = resultTagItem.HeightRatio;

                    BinCardIn.BinCard_HeightSign = resultTagItem.Height;
                    BinCardIn.BinCard_HeightSign_Index = resultTagItem.Height_Index;
                    BinCardIn.BinCard_HeightSign_Id = resultTagItem.Height_Id;
                    BinCardIn.BinCard_HeightSign_Name = resultTagItem.Height_Name;
                    BinCardIn.BinCard_HeightSignRatio = resultTagItem.HeightRatio;

                    BinCardIn.BinCard_UnitVolumeSign = resultTagItem.UnitVolume;
                    BinCardIn.BinCard_VolumeSign = resultTagItem.Volume;

                    #endregion

                    BinCardIn.Ref_Document_No = resultGRL.FirstOrDefault().goodsReceive_No;
                    BinCardIn.Ref_Document_Index = resultGRL.FirstOrDefault().goodsReceive_Index;
                    BinCardIn.Ref_DocumentItem_Index = resultGRL.FirstOrDefault().goodsReceiveItem_Index;
                    BinCardIn.Create_By = data.create_By;
                    BinCardIn.Create_Date = DateTime.Now;
                    BinCardIn.BinBalance_Index = BinBalanceOld.BinBalance_Index;
                    BinCardIn.ERP_Location = item.erp_Location;
                    BinCardIn.ERP_Location_To = item.erp_Location;

                    db.wm_BinCard.Add(BinCardOut);
                    db.wm_BinCard.Add(BinCardIn);

                    #endregion

                    #region Update StatusTaskGRItem

                    State = "SavePutaway update taskGRItem";

                    var TaskModel = new TaskfilterViewModel();
                    TaskModel.tag_No = data.tag_No;
                    TaskModel.taskGR_No = data.taskGR_No;

                    var resultTask = new TaskfilterViewModel();
                    resultTask = utils.SendDataApi<TaskfilterViewModel>(new AppSettingConfig().GetUrl("CheckTagTask"), TaskModel.sJson());
                    TaskGRIndex = resultTask.taskGR_Index;

                    var TaskGRItemOld = db2.im_TaskGRItem.Find(resultTask.taskGRItem_Index);

                    TaskGRItemOld.Document_Status = 1;
                    TaskGRItemOld.Update_By = data.create_By;
                    TaskGRItemOld.Update_Date = DateTime.Now;

                    #endregion

                    #region Update im_GoodsReceiveItemLocation putaway location

                    State = "SavePutaway update TagItem";

                    var GRIL = db2.im_GoodsReceiveItemLocation.Where(c => c.TagItem_Index == item.tagItem_Index && c.Tag_Index == item.tag_Index);

                    foreach (var g in GRIL)
                    {
                        var GRILOld = db2.im_GoodsReceiveItemLocation.Find(g.GoodsReceiveItemLocation_Index);

                        GRILOld.PutawayLocation_Index = data.confirm_Location_Index;
                        GRILOld.PutawayLocation_Id = data.confirm_Location_Id;
                        GRILOld.PutawayLocation_Name = data.confirm_Location_Name;
                        GRILOld.Update_By = data.create_By;
                        GRILOld.Update_Date = DateTime.Now;
                        GRILOld.Putaway_By = data.create_By;
                        GRILOld.Putaway_Date = DateTime.Now;
                        GRILOld.Putaway_Status = 1;

                    }

                    #endregion

                    #region Update StatusTagItem

                    State = "SavePutaway update TagItem";

                    var TagItemOld = dbInbound.wm_TagItem.Find(item.tagItem_Index);

                    TagItemOld.Tag_Status = 2;
                    TagItemOld.Update_By = data.create_By;
                    TagItemOld.Update_Date = DateTime.Now;

                    #endregion
                }

                var transaction = db.Database.BeginTransaction(IsolationLevel.Serializable);
                var transactionx = db2.Database.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    db.SaveChanges();
                    db2.SaveChanges();
                    transaction.Commit();
                    transactionx.Commit();
                    // 
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SavePutaway", msglog);
                    transaction.Rollback();
                    transactionx.Rollback();
                    throw exy;
                }

                var transUpdateTagItem = dbInbound.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    dbInbound.SaveChanges();
                    transUpdateTagItem.Commit();
                }

                catch (Exception exeption)
                {
                    msglog = State + " ex Rollback " + exeption.Message.ToString();
                    olog.logging("SavePutaway TagItem", msglog);
                    transUpdateTagItem.Rollback();
                    throw exeption;
                }

                #region CheckTaskSuccess
                var CheckTaskModel = new TaskfilterViewModel();
                var resultCheckTask = new List<TaskfilterViewModel>();

                CheckTaskModel.taskGR_No = data.taskGR_No;
                resultCheckTask = utils.SendDataApi<List<TaskfilterViewModel>>(new AppSettingConfig().GetUrl("CheckTaskSuccess"), CheckTaskModel.sJson());

                if (resultCheckTask.Count <= 0)
                {
                    var TaskGROld = db2.im_TaskGR.Find(TaskGRIndex);

                    TaskGROld.Document_Status = 2;
                    TaskGROld.Update_By = data.create_By;
                    TaskGROld.Update_Date = DateTime.Now;

                    var GROld = db2.im_GoodsReceive.Find(GRIndex);

                    if (GROld != null)
                    {
                        GROld.Document_Status = 4;
                    }

                    var transaction2 = db2.Database.BeginTransaction(IsolationLevel.Serializable);

                    try
                    {
                        db2.SaveChanges();
                        transaction2.Commit();
                        return "TaskSuccess";
                    }

                    catch (Exception exy)
                    {
                        msglog = State + " ex Rollback " + exy.Message.ToString();
                        olog.logging("SaveUpdateTaskPutaway", msglog);
                        transaction.Rollback();
                        transactionx.Rollback();
                        throw exy;
                    }

                }
                #endregion


                return "Done";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String SavePutawayAuto()
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();


            try
            {
                var Models = new LPNItemViewModel();
                var queryView = db.View_InsertPutaway.AsQueryable();
                string msgretrun = "";
                var listdataView = queryView.ToList();

                foreach (var item in listdataView)
                {
                    Models.isSku = false;
                    Models.tag_No = item.Tag_No;
                    Models.taskGR_No = item.TaskGR_No;
                    Models.suggest_Location_Name = item.Suggest_Location_Name;
                    Models.goodsReceive_Index = (Guid)item.GoodsReceive_Index;
                    Models.tag_Index = item.Tag_Index;
                    Models.confirm_Location_Name = item.confirm_Location_Name;
                    Models.confirm_Location_Index = (Guid)item.confirm_Location_Index;
                    Models.confirm_Location_Id = item.confirm_Location_Id;
                    Models.create_By = item.create_By;

                    msgretrun += item.Tag_No + " : ";
                    msgretrun += SavePutaway(Models);
                    msgretrun += Environment.NewLine;
                }




                return msgretrun;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BinBalanceViewModel> scanSKU(BinBalanceViewModel data)
        {
            try
            {
                var result = new List<BinBalanceViewModel>();

                var query = db.wm_BinBalance.AsQueryable();

                if (data.goodsReceive_Index != new Guid("00000000-0000-0000-0000-000000000000".ToString()))
                {
                    query = query.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index);
                }
                if (!string.IsNullOrEmpty(data.tag_No))
                {
                    query = query.Where(c => c.Tag_No.Contains(data.tag_No));
                }
                if (!string.IsNullOrEmpty(data.product_Id))
                {
                    query = query.Where(c => c.Product_Id.Contains(data.product_Id));
                }

                var queryresult = query.GroupBy(c => new { c.Product_Id, c.ProductConversion_Name, c.Product_Index })
                    .Select(c => new { SumQty = c.Sum(s => s.BinBalance_QtyBal), c.Key.ProductConversion_Name, c.Key.Product_Index }).ToList();

                foreach (var item in queryresult)
                {
                    var resultItem = new BinBalanceViewModel();

                    resultItem.binBalance_QtyBal = item.SumQty;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.product_Index = item.Product_Index;

                    result.Add(resultItem);

                }

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        public actionResultViewModel filterTaskPallet(View_TaskGRViewModel model)
        {
            try
            {
                var actionResult = new actionResultViewModel();

                var query = db2.View_TaskGR_Pallet.AsQueryable();

                query = query.Where(c => c.Document_Status != 2 && c.IsScanDockToStaging ==1 && c.IsPallet_Inspection == 0);


                //if (!string.IsNullOrEmpty(model.key))
                //{
                //    query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                //}


                var findTag = db2.wm_TagItem.Where(c => c.Tag_No == model.key).FirstOrDefault();

                if (findTag != null)
                {
                    //var findTagItem = db2.wm_TagItem.Where(c => c.Tag_Index == findTag.Tag_Index).FirstOrDefault();

                    //var findGR = db2.im_GoodsReceive.Where(c => c.GoodsReceive_Index == findTag.GoodsReceive_Index).FirstOrDefault();


                    query = query.Where(c => c.Tag_No == findTag.Tag_No);
                    //query = query.Where(c => c.Ref_Document_No == findGR.GoodsReceive_No);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                    }
                }

                var Item = new List<View_TaskGR_Pallet>();
                var TotalRow = new List<View_TaskGR_Pallet>();

                TotalRow = query.ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                Item = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();


                var result = new List<View_TaskGRViewModel>();

                foreach (var data in Item)
                {
                    var resultItem = new View_TaskGRViewModel();

                    resultItem.taskGR_Index = data.TaskGR_Index;
                    resultItem.taskGR_No = data.TaskGR_No;
                    resultItem.ref_Document_Index = data.Ref_Document_Index;
                    resultItem.ref_Document_No = data.Ref_Document_No;
                    resultItem.create_Date = data.Create_Date.toString();
                    result.Add(resultItem);

                }
                var count = TotalRow.Count;

                actionResult.itemsTaskPutaway = result.OrderByDescending(o => o.create_Date).ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };
                return actionResult;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public actionResultViewModel filterTask(View_TaskGRViewModel model)
        {
            try
            {
                var actionResult = new actionResultViewModel();

                var query = db2.View_TaskGR_Putaway.AsQueryable();

                query = query.Where(c => c.Document_Status != 2 && c.IsScanDockToStaging == 1 && c.IsPallet_Inspection == 1);


                //if (!string.IsNullOrEmpty(model.key))
                //{
                //    query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                //}


                var findTag = db2.wm_TagItem.Where(c => c.Tag_No == model.key).FirstOrDefault();

                if (findTag != null)
                {
                    //var findTagItem = db2.wm_TagItem.Where(c => c.Tag_Index == findTag.Tag_Index).FirstOrDefault();

                    //var findGR = db2.im_GoodsReceive.Where(c => c.GoodsReceive_Index == findTag.GoodsReceive_Index).FirstOrDefault();


                    query = query.Where(c => c.Tag_No == findTag.Tag_No);
                    //query = query.Where(c => c.Ref_Document_No == findGR.GoodsReceive_No);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                    }
                }

                var Item = new List<View_TaskGR_Putaway>();
                var TotalRow = new List<View_TaskGR_Putaway>();

                TotalRow = query.ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                Item = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();


                var result = new List<View_TaskGRViewModel>();

                foreach (var data in Item)
                {
                    var resultItem = new View_TaskGRViewModel();

                    resultItem.taskGR_Index = data.TaskGR_Index;
                    resultItem.taskGR_No = data.TaskGR_No;
                    resultItem.ref_Document_Index = data.Ref_Document_Index;
                    resultItem.ref_Document_No = data.Ref_Document_No;
                    resultItem.create_Date = data.Create_Date.toString();
                    result.Add(resultItem);

                }
                var count = TotalRow.Count;

                actionResult.itemsTaskPutaway = result.OrderByDescending(o => o.create_Date).ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };
                return actionResult;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public actionResultViewModel filterTaskunPack(View_TaskGRViewModel model)
        {
            try
            {
                var actionResult = new actionResultViewModel();

                var query = db2.View_TaskGR.AsQueryable();

                query = query.Where(c => c.Document_Status != 2);


                //if (!string.IsNullOrEmpty(model.key))
                //{
                //    query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                //}


                var findTag = db2.wm_TagItem.Where(c => c.Tag_No == model.key).FirstOrDefault();

                if (findTag != null)
                {
                    //var findTagItem = db2.wm_TagItem.Where(c => c.Tag_Index == findTag.Tag_Index).FirstOrDefault();

                    //var findGR = db2.im_GoodsReceive.Where(c => c.GoodsReceive_Index == findTag.GoodsReceive_Index).FirstOrDefault();


                    query = query.Where(c => c.Tag_No == findTag.Tag_No);
                    //query = query.Where(c => c.Ref_Document_No == findGR.GoodsReceive_No);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                    }
                }

                var Item = new List<View_TaskGR>();
                var TotalRow = new List<View_TaskGR>();

                TotalRow = query.ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                Item = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();


                var result = new List<View_TaskGRViewModel>();

                foreach (var data in Item)
                {
                    var resultItem = new View_TaskGRViewModel();

                    resultItem.taskGR_Index = data.TaskGR_Index;
                    resultItem.taskGR_No = data.TaskGR_No;
                    resultItem.ref_Document_Index = data.Ref_Document_Index;
                    resultItem.ref_Document_No = data.Ref_Document_No;
                    resultItem.create_Date = data.Create_Date.toString();
                    result.Add(resultItem);

                }
                var count = TotalRow.Count;

                actionResult.itemsTaskPutaway = result.OrderByDescending(o => o.create_Date).ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };
                return actionResult;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region ReportPutawayGR
        public dynamic ReportPutawayGR(ReportPutawayGRViewModel data, string rootPath = "")
        {
            var IB_DBContext = new InboundDataAccessDbContext();

            var culture = new System.Globalization.CultureInfo("en-US");
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();

            var result = new List<ReportPutawayGRViewModel>();

            try
            {
                var queryGR = IB_DBContext.View_ReportPutawayGR.Where(c => c.GoodsReceive_Index == data.goodsReceive_Index);

                var query = queryGR.ToList();
                var count = 1;
                foreach (var item in query)
                {

                    string date = item.GoodsReceive_Date.toString();
                    string GRDate = DateTime.ParseExact(date.Substring(0, 8), "yyyyMMdd",
                    System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy", culture);

                    var resultItem = new ReportPutawayGRViewModel();

                    resultItem.goodsReceive_No = item.GoodsReceive_No;
                    resultItem.goodsReceive_Date = GRDate;
                    resultItem.product_Id = item.Product_Id;
                    resultItem.product_Name = item.Product_Name;
                    resultItem.productConversion_Id = item.ProductConversion_Id;
                    resultItem.productConversion_Name = item.ProductConversion_Name;
                    resultItem.owner_Id = item.Owner_Id;
                    resultItem.owner_Name = item.Owner_Name;
                    resultItem.tag_No = item.Tag_No;
                    resultItem.qty = item.Qty;
                    resultItem.itemStatus_Id = item.ItemStatus_Id;
                    resultItem.itemStatus_Name = item.ItemStatus_Name;
                    resultItem.location_Id = item.Location_Id;
                    resultItem.location_Name = item.Location_Name;
                    resultItem.RowCount = count;

                    result.Add(resultItem);
                    count = count + 1;
                }


                rootPath = rootPath.Replace("\\PutawayAPI", "");
                //var reportPath = rootPath + "\\PutawayBusiness\\Reports\\ReportPutawayGR.rdlc";
                //var reportPath = rootPath + "\\Reports\\ReportPutawayGR.rdlc";
                var reportPath = rootPath + new AppSettingConfig().GetUrl("ReportPutawayGR");
                LocalReport report = new LocalReport(reportPath);
                report.AddDataSource("DataSet1", result);

                System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                string fileName = "";
                string fullPath = "";
                fileName = "tmpReport" + DateTime.Now.ToString("yyyyMMddHHmmss");

                var renderedBytes = report.Execute(RenderType.Pdf);

                Utils objReport = new Utils();
                fullPath = objReport.saveReport(renderedBytes.MainStream, fileName + ".pdf", rootPath);
                var saveLocation = objReport.PhysicalPath(fileName + ".pdf", rootPath);
                return saveLocation;


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region AutobasicSuggestion
        public List<ItemListViewModel> autobasicSuggestion(ItemListViewModel data)
        {
            var items = new List<ItemListViewModel>();
            try
            {
                var query = db2.View_TaskGR.AsQueryable();

                if (!string.IsNullOrEmpty(data.key))
                {
                    query = query.Where(c => c.TaskGR_No.Contains(data.key));
                }

                query = query.Where(c => c.Document_Status != 2);


                var result = query.Select(c => new { c.TaskGR_No }).Distinct().Take(10).Select(s => new ItemListViewModel
                {
                    name = s.TaskGR_No,
                    key = s.TaskGR_No

                }).Distinct();
                var result2 = db2.View_TaskGR.Where(c => c.Ref_Document_No.Contains(data.key)).Select(c => new { c.Ref_Document_No }).Distinct().Take(10).Select(s => new ItemListViewModel
                {
                    name = s.Ref_Document_No,
                    key = s.Ref_Document_No

                }).Distinct();

                var result3 = db2.wm_Tag.Where(c => c.Tag_No.Contains(data.key)).Select(c => new { c.Tag_No }).Distinct().Take(10).Select(s => new ItemListViewModel
                {
                    name = s.Tag_No,
                    key = s.Tag_No
                }).Distinct();
                //foreach (var item in result)
                //{
                //    var resultItem = new ItemListViewModel
                //    {
                //        name = item.TaskGR_No,
                //        key = item.TaskGR_No
                //    };
                //    items.Add(resultItem);
                //}

                items = result.Union(result2).Union(result3).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return items;
        }

        #endregion

        #region CheckUserassign
        public actionResultViewModel checkUserassign(View_TaskGRViewModel model)
        {
            try
            {
                String State = "Start";
                String msglog = "";
                var olog = new logtxt();

                var actionResult = new actionResultViewModel();

                if (model.update == 0)
                {

                    var query = db2.im_TaskGR.Where(c => c.TaskGR_No == model.taskGR_No && c.UserAssign == model.userAssign).FirstOrDefault();

                    if (query != null)
                    {
                        var task = db2.im_TaskGR.Find(query.TaskGR_Index);
                        task.UserAssign = model.userAssign;
                        actionResult.msg = true;
                    }
                    else
                    {
                        var query2 = db2.im_TaskGR.Where(c => c.TaskGR_No == model.taskGR_No).FirstOrDefault();

                        if (query2.UserAssign == "" || query2.UserAssign == null)
                        {
                            var task = db2.im_TaskGR.Find(query2.TaskGR_Index);
                            task.UserAssign = model.userAssign;
                            actionResult.msg = true;
                        }
                        else
                        {
                            actionResult.msg = false;
                            return actionResult;
                        }

                    }
                }


                if (model.update == 1)
                {
                    var query = db2.im_TaskGR.Where(c => c.TaskGR_No == model.taskGR_No).FirstOrDefault();
                    if (query != null)
                    {
                        var task = db2.im_TaskGR.Find(query.TaskGR_Index);
                        task.UserAssign = model.userAssign;
                    }
                }

                if (model.update == 2)
                {

                    var query = db2.im_TaskGR.Where(c => c.TaskGR_No == model.taskGR_No && c.UserAssign == model.userAssign).FirstOrDefault();

                    if (query == null)
                    {
                        actionResult.msg = false;
                        return actionResult;
                    }
                    else
                    {
                        actionResult.msg = true;

                    }
                }



                var transactionx = db2.Database.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    db2.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("UpdateUserAssign", msglog);
                    transactionx.Rollback();
                    throw exy;
                }



                return actionResult;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region deleteUserassign
        public actionResultViewModel deleteUserassign(View_TaskGRViewModel model)
        {
            try
            {
                String State = "Start";
                String msglog = "";
                var olog = new logtxt();

                var actionResult = new actionResultViewModel();

                var query = db2.im_TaskGR.Where(c => c.TaskGR_No == model.taskGR_No).FirstOrDefault();
                if (query != null)
                {
                    var task = db2.im_TaskGR.Find(query.TaskGR_Index);
                    task.UserAssign = "";
                }

                var transactionx = db2.Database.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    db2.SaveChanges();
                    transactionx.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("UpdateUserAssign", msglog);
                    transactionx.Rollback();
                    throw exy;
                }



                return actionResult;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region DockToStaging
        public List<LPNItemViewModel> scanLpnDockToStaging(LPNItemViewModel data)
        {

            try
            {
                var filterModel = new LPNItemViewModel();
                var result = new List<LPNItemViewModel>();
                var resultErorr = new List<LPNItemViewModel>();


                filterModel.tag_No = data.tag_No;

                //GetConfig
                result = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("TagItemFilter"), filterModel.sJson());

                if (result.Count > 0)
                {
                    var TaskModel = new TaskfilterViewModel();
                    TaskModel.tag_No = data.tag_No;
                    TaskModel.taskGR_No = data.taskGR_No;

                    var resultTask = new TaskfilterViewModel();
                    resultTask = utils.SendDataApi<TaskfilterViewModel>(new AppSettingConfig().GetUrl("CheckTagTask"), TaskModel.sJson());

                    if (resultTask == null)
                    {
                        return resultErorr;

                    }

                }
                else
                {
                    return resultErorr;
                }

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LocationConfigViewModel> chkLocation(LocationConfigViewModel data)
        {

            try
            {
                //bool result = false;

                var filterModel = new LocationConfigViewModel();
                var result = new List<LocationConfigViewModel>();

                var modelLocation = new
                {
                    location_Name = data.location_Name
                };

                //filterModel.location_Name = data.location_Name;

                //GetConfig
                if (data.location_Name != null)
                {
                    result = utils.SendDataApi<List<LocationConfigViewModel>>(new AppSettingConfig().GetUrl("getLocationV2"), modelLocation.sJson());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LocationConfigViewModel> chkLocationDockToStaging(LocationConfigViewModel data)
        {

            try
            {
                //bool result = false;

                var filterModel = new LocationConfigViewModel();
                var result = new List<LocationConfigViewModel>();

                var modelLocation = new
                {
                    location_Name = data.location_Name
                    ,locationType_Index = "A1F7BFA0-1429-4010-863D-6A0EB01DB61D" // Location Type Dock to Staging
                };

                //filterModel.location_Name = data.location_Name;

                //GetConfig
                if (data.location_Name != null)
                {
                    result = utils.SendDataApi<List<LocationConfigViewModel>>(new AppSettingConfig().GetUrl("getLocationV2"), modelLocation.sJson());
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool chkTagItem(LPNItemViewModel data)
        {

            try
            {
                var result = false;
                var filterModel = new LPNItemViewModel();
                var resultTagINPutaway = new List<LPNItemViewModel>();
                var resultTagBinBalance = new List<BinBalanceViewModel>();
                var resultTag = new BinBalanceViewModel();

                filterModel.tag_No = data.tag_No;

                resultTag = utils.SendDataApi<List<BinBalanceViewModel>>(new AppSettingConfig().GetUrl("getBinbalance"), filterModel.sJson()).FirstOrDefault(); // Not in BUF-IP

                if (resultTag != null)
                {
                    if(resultTag.location_Id == "902")
                    {
                        result = true;
                    }
                    //else
                    //{
                    //    resultTagINPutaway = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("TagItemFilter"), filterModel.sJson());
                    //    // ยังไม่ได้ Putaway จะต้องมีข้อมูล
                    //    if (resultTagINPutaway.Count == 0)
                    //    {
                    //        result = "Can not Putaway";
                    //    }
                    //}
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String SaveDockToStaging(LPNItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            Guid? TaskGRIndex = new Guid();
            Guid? GRIndex = new Guid();
            try
            {
                State = "SaveDockToStaging update TagItem";
                var filterModel = new LPNItemViewModel();
                var result = new List<LPNItemViewModel>();

                if (string.IsNullOrEmpty(data.tag_No))
                {
                    olog.logging("SaveDockToStaging", "TAG NO  DATA");
                    return "NOTAG";
                }
                if (data.isSku == false)
                {
                    filterModel.tag_No = data.tag_No;
                }

                else
                {
                    filterModel.tag_No = data.tag_No;
                    filterModel.product_Index = data.product_Index;
                }

                State = "SavePutaway find TagItem";

                result = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("TagItemFilter"), filterModel.sJson());

                foreach (var item in result)
                {
                    var TagItemOld = db2.wm_TagItem.Find(item.tagItem_Index);

                    TagItemOld.IsScanDockToStaging = 1;
                    TagItemOld.Location_Index = data.confirm_Location_Index;
                    TagItemOld.Location_Id = data.confirm_Location_Id;
                    TagItemOld.Location_Name = data.confirm_Location_Name;
                    TagItemOld.UpdateDockToStaging_By = data.create_By;
                    TagItemOld.UpdateDockToStaging_Date = DateTime.Now;
                }

                var transaction = db2.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db2.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveDockToStaging", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                return "Done";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String SavetypeselectivePallet(LPNItemViewModel data)
        {
            String State = "Start";
            String msglog = "";
            var olog = new logtxt();
            try
            {
                State = "SavetypeselectivePallet update TagItem";
                var filterModel = new LPNItemViewModel();
                var result = new List<LPNItemViewModel>();

                if (data.isSku == false)
                {
                    filterModel.tag_No = data.tag_No;
                }

                else
                {
                    filterModel.tag_No = data.tag_No;
                    filterModel.product_Index = data.product_Index;
                }

                State = "SavePutaway find TagItem";

                result = utils.SendDataApi<List<LPNItemViewModel>>(new AppSettingConfig().GetUrl("TagItemFilter"), filterModel.sJson());

                foreach (var item in result)
                {
                    var TagItemOld = db2.wm_TagItem.Find(item.tagItem_Index);

                    TagItemOld.IsPallet_Inspection = 1;
                    TagItemOld.UpdatePallet_Inspection_By = data.create_By;
                    TagItemOld.UpdatePallet_Inspection_Date = DateTime.Now;
                }

                var transaction = db2.Database.BeginTransaction(IsolationLevel.Serializable);
                try
                {
                    db2.SaveChanges();
                    transaction.Commit();
                }

                catch (Exception exy)
                {
                    msglog = State + " ex Rollback " + exy.Message.ToString();
                    olog.logging("SaveDockToStaging", msglog);
                    transaction.Rollback();
                    throw exy;
                }

                return "Done";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public actionResultViewModel filterTaskDockToStaging(View_TaskGRViewModel model)
        {
            try
            {
                var actionResult = new actionResultViewModel();

                var query = db2.View_TaskGR.AsQueryable();

                query = query.Where(c => c.Document_Status != 2 && c.IsScanDockToStaging == 0);


                //if (!string.IsNullOrEmpty(model.key))
                //{
                //    query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                //}


                var findTag = db2.wm_TagItem.Where(c => c.Tag_No == model.key).FirstOrDefault();

                if (findTag != null)
                {
                    //var findTagItem = db2.wm_TagItem.Where(c => c.Tag_Index == findTag.Tag_Index).FirstOrDefault();

                    var findGR = db2.im_GoodsReceive.Where(c => c.GoodsReceive_Index == findTag.GoodsReceive_Index).FirstOrDefault();


                    //query = query.Where(c => c.Tag_No == findTag.Tag_No);
                    query = query.Where(c => c.Ref_Document_No == findGR.GoodsReceive_No);
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.key))
                    {
                        query = query.Where(c => c.TaskGR_No.Contains(model.key) || c.Ref_Document_No.Contains(model.key));
                    }
                }

                var Item = new List<View_TaskGR>();
                var TotalRow = new List<View_TaskGR>();

                TotalRow = query.ToList();


                if (model.CurrentPage != 0 && model.PerPage != 0)
                {
                    query = query.Skip(((model.CurrentPage - 1) * model.PerPage));
                }

                if (model.PerPage != 0)
                {
                    query = query.Take(model.PerPage);

                }

                Item = query.OrderByDescending(o => o.Create_Date).ThenByDescending(o => o.Create_Date).ToList();


                var result = new List<View_TaskGRViewModel>();

                foreach (var data in Item)
                {
                    var resultItem = new View_TaskGRViewModel();

                    resultItem.taskGR_Index = data.TaskGR_Index;
                    resultItem.taskGR_No = data.TaskGR_No;
                    resultItem.ref_Document_Index = data.Ref_Document_Index;
                    resultItem.ref_Document_No = data.Ref_Document_No;
                    resultItem.create_Date = data.Create_Date.toString();
                    result.Add(resultItem);

                }
                var count = TotalRow.Count;

                actionResult.itemsTaskPutaway = result.OrderByDescending(o => o.create_Date).ToList();
                actionResult.pagination = new Pagination() { TotalRow = count, CurrentPage = model.CurrentPage, PerPage = model.PerPage, };
                return actionResult;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


    }
}
