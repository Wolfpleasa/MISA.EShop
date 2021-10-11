<template>
  <div @click="myClick()">
    <Header :mode="mode" :showFormDetail="!hideFormDetail" />
    <Menu />
    <div class="content">
      <ToolBar
        :productId="productId"
        @btnAddOnClick="btnAddOnClick"
        @btnDuplicateOnClick="btnDuplicateOnClick"
        @btnEditOnClick="btnEditOnClick"
        @btnDeleteOnClick="btnDeleteOnClick"
        @refreshOnClick="refreshOnClick"
      />
      <Table
        :response="response"
        :count="count"
        @getId="getId"
        @refreshOnClick="refreshOnClick"
        @btnEditOnClick="btnEditOnClick"
      />
    </div>

    <ProductDetail
      v-bind:dnone="hideFormDetail"
      :productId="productId"
      :formMode="formMode"
      :reopen="reopen"
      @btnDialogCancelOnClick="btnDialogCancelOnClick"
      @resetAfterSaveData="resetAfterSaveData"
      @callToastMessage="callToastMessage"
      @callAlertPopup="callAlertPopup"
      @hidePopupInfo="hidePopupInfo"
    />

    <!-- 3 page làm mờ -->
    <!--  của warningpopup -->
    <div
      :class="['pageCover z-index-11', { 'd-none': hideWarningPopup }]"
    ></div>

    <!--  của warningpopup -->
    <div :class="['pageCover z-index-11', { 'd-none': hideAlertPopup }]"></div>

    <!--  của popup thêm thông tin đơn vị,nhóm hàng hóa -->
    <div :class="['pageCover z-index-11', { 'd-none': hidePopup }]"></div>

    <!-- của loader  -->
    <div :class="['pageCover z-index-1', { 'd-none': hideLoader }]"></div>
    <Loader :hideLoader="hideLoader" />

    <WarningPopup
      :idPopup="idPopup"
      :dnone="hideWarningPopup"
      :warningText="warningText"
      :warning="warning"
      :subClass="bonusClass"
      @btnCancelOnClick="btnCancelOnClick"
      @btnConfirmOnClick="btnConfirmOnClick"
    />

    <ToastMessage
      :subClass="subClass"
      :hideToastMessage="hideToastMessage"
      :toastMessageText="toastMessageText"
      @closeToastMessage="closeToastMessage"
    />

    <AlertPopup
      :subClass="subClass"
      :hideAlertPopup="hideAlertPopup"
      :alertMessageText="alertMessageText"
    />

    <Popup
      :dnone="hidePopup"
      :property="property"
      @closePopupInfo="closePopupInfo"
    />
  </div>
</template>

<script>
import { eventBus } from "../../main.js";
import axios from "axios";

import Constant from "../../common/constant1.js";
import Enumeration from "../../common/enumeration.js";
import ResourceVN from "../../common/resourceVN.js";

import Header from "../../components/layout/TheHeader.vue";
import Menu from "../../components/layout/TheMenu.vue";
import ToolBar from "../../components/base/BaseToolBar1.vue";
import Table from "../../components/base/BaseTable.vue";
import AlertPopup from "../../components/base/BaseAlertPopup.vue";
import ToastMessage from "../../components/base/BaseToastMessage.vue";
import Loader from "../../components/base/BaseLoader.vue";
import ProductDetail from "./ProductDetail.vue";
import WarningPopup from "../../components/layout/ThePopup.vue";
import Popup from "../../components/base/BasePopup.vue";
export default {
  name: "ProductList",
  components: {
    Header,
    Menu,
    ToolBar,
    Loader,
    ToastMessage,
    ProductDetail,
    WarningPopup,
    Table,
    AlertPopup,
    Popup,
  },
  data() {
    return {
      //ProductList
      product: {},
      mode: "",

      //ToastMessage
      hideToastMessage: true,
      toastMessageText: "",

      //Loader
      hideLoader: true,

      //AlertPopup
      alertMessageText: "",
      hideAlertPopup: true,

      property: "",

      //ProductDetail
      hideFormDetail: true,
      productId: null,
      reopen: true,
      formMode: Enumeration.FormMode.None,
      subClass: "",
      hidePopup: true,

      //--------------- form cảnh báo/thông báo ------------------
      hideWarningPopup: true,
      warning: "",
      warningText: "",
      idPopup: "",
      btnCancelText: "",
      btnConfirmText: "",
      notifyMode: -1,
      response: "",
      bonusClass: "",

      count: 0,
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện click trong trang hàng hóa
     * Created By: Ngọc 01/10/2021
     */
    myClick() {
      this.count++;
    },

    /**
     * Hàm ẩn hiện loader của popup thêm đơn vị, nhóm hàng hóa
     * Created By: Ngọc 01/10/2021
     */
    closePopupInfo() {
      this.hidePopup = true;
      this.property = "";
    },

    /**
     * Hàm mở và thêm thông tin cơ bản
     * Created By: Ngọc 01/10/2021
     */
    hidePopupInfo(hidePopup, fieldName) {
      this.hidePopup = hidePopup;
      this.property = fieldName;
    },

    /**
     * Hàm lấy id của hàng đang được chọn
     * Created By: Ngọc 28/09/2021
     */
    getId(productId) {
      this.productId = productId;
    },

    /**
     * Hàm Ẩn/hiện Loader
     * Ngọc 12/8/2021
     */
    refreshOnClick() {
      this.hideLoader = false;
      setTimeout(() => {
        this.hideLoader = true;
      }, 1000);
    },

    /**
     * Hàm mở popup
     * Created By: Ngọc 25/09/2021
     */
    btnAddOnClick() {
      let me = this;
      me.hideFormDetail = false;
      this.formMode = Enumeration.FormMode.Add;
      this.mode = "Thêm mới";
    },

    /**
     * Hàm mở form detail để sửa
     * Created By: Ngọc 25/09/2021
     */
    btnEditOnClick() {
      setTimeout(() => {
        this.hideFormDetail = false;
        this.formMode = Enumeration.FormMode.Edit;
        this.mode = "Sửa";
      }, 10);
    },

    /**
     * Hàm bấm checkbox ở td được BaseCheckBox gửi lên
     * Ngọc 1/8/2021
     */
    clickCheckboxTd(index) {
      let me = this;
      me.isSelected[index] = !me.isSelected[index];
      me.HideBtnDelete = false;
      me.HideBtnDuplicate = false;
      setTimeout(function () {
        me.checked = me.CheckAllCBTd();
      }, 10);
    },

    /**
     * Hàm kiểm tra tất cả checkbox của td có được chọn không
     * Ngọc 1/8/2021
     */
    CheckAllCBTd() {
      let me = true;
      for (var i = 0; i < this.isSelected.length; i++) {
        if (!this.isSelected[i]) {
          me = false;
          break;
        }
      }
      return me;
    },

    /**
     * Hàm bấm checkbox ở th được BaseCheckBox gửi lên
     * Ngọc 1/8/2021
     */
    clickCheckboxTh() {
      this.checked = !this.checked;
      if (this.checked) {
        this.isSelected.fill(true);
        this.HideBtnDelete = false;
        this.HideBtnDuplicate = false;
      } else {
        this.isSelected.fill(false);
        this.HideBtnDelete = true;
        this.HideBtnDuplicate = true;
      }
    },

    /**
     * Hàm bấm nút xóa
     * Created By: Ngọc 30/10/2021
     */
    async btnDeleteOnClick() {
      let me = this;
      await axios
        .get(`${Constant.LocalUrl}/Products/${me.productId}`)
        .then((res) => {
          res.data.forEach((data) => {
            if (data.ProductId == me.productId) {
              Object.assign(me.product, data);
            }
          });
        })
        .catch((err) => {
          console.log(err);
        });
      me.warningText = `${ResourceVN.CONFIRM_DELETE} <span>${me.product.SKUCode}-${me.product.ProductName}</span> ${ResourceVN.CONFIRM}`;
      me.warning = `Xóa hàng hóa`;

      me.hideWarningPopup = false;
      me.idPopup = ResourceVN.Popup.Warning;
    },

    /**
     * Hàm bấm nút nhân bản
     * Created By: Ngọc 30/10/2021
     */
    btnDuplicateOnClick() {
      this.hideFormDetail = false;
      this.formMode = Enumeration.FormMode.Duplicate;
      this.mode = "Nhân bản";
    },

    /**
     * Hàm bấm nút xác nhận
     * Created By: Ngọc 30/10/2021
     */
    async btnConfirmOnClick(idPopup) {
      let me = this;
      // Nếu form cảnh báo xóa được gọi
      me.hideWarningPopup = true;
      if (idPopup == ResourceVN.Popup.Warning) {
        await axios
          .delete(`${Constant.LocalUrl}/Products/${me.productId}`)
          .then(() => {
            me.callToastMessage("Xóa dữ liệu thành công", "message-green");
            me.isSelected = [];
          })
          .catch(() => {
            me.callToastMessage(
              "Có vấn đề xảy ra, không thể xóa dữ liệu",
              "message-red"
            );
          });
        let responseTime = Number(new Date());
        me.response = `${responseTime}`;
        me.refreshOnClick();
      }

      me.idPopup = "";
    },

    /**
     * Hàm lấy mã hàng hóa mới
     * Ngọc 6/8/2021
     */
    getNewCode() {
      return new Promise((resolve) => {
        axios
          .get(`${Constant.LocalUrl}Products/NewProductCode`)
          .then((res) => {
            console.log(res.data, Number(new Date()));
            resolve(res.data);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Hàm lấy hàng hóa theo id hàng hóa
     * Ngọc 6/8/2021
     */
    getInfo(productId) {
      return new Promise((resolve) => {
        axios
          .get(`${Constant.LocalUrl}Products/${productId}`)
          .then((res) => {
            let newProduct = res.data;
            console.log(newProduct.FullName, Number(new Date()));
            resolve(newProduct);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Thêm hàng hóa được nhân bản
     * Ngọc 6/8/2021
     */
    InsertDuplicate(productDuplicated) {
      return new Promise((resolve) => {
        axios
          .post(`${Constant.LocalUrl}Products/`, productDuplicated)
          .then(() => {
            resolve(1);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Hàm đóng popup cảnh báo xóa
     * Created By: Ngọc 30/10/2021
     */
    btnCancelOnClick() {
      let me = this;
      me.hideWarningPopup = true;
    },

    /**
     * Hàm bấm hủy bỏ ở form detail
     * Created By: Ngọc 22/09/2021
     */
    btnDialogCancelOnClick() {
      let me = this;
      me.hideFormDetail = true;
      me.mode = "";
      me.formMode = Enumeration.FormMode.None;
    },

    /**
     * Hàm thực hiện những reset sau khi lưu
     * Created By: Ngọc 25/09/2021
     */
    resetAfterSaveData() {
      let me = this;
      //reset formMode
      me.formMode = Enumeration.FormMode.None;
      //Đống form thêm/sửa
      me.hideFormDetail = true;
      //Đóng popup
      me.hideWarningPopup = true;
      let responseTime = Number(new Date());
      me.response = `${responseTime}`;
      me.refreshOnClick();
    },

    /**
     * Hàm hiển thị toastmessage
     * Ngọc 2/8/2021
     */
    callToastMessage(text, subClass) {
      this.hideToastMessage = false;
      this.toastMessageText = text;
      this.subClass = subClass;
    },

    /**
     * Hàm hiển thị toastmessage
     * Created By: Ngọc 04/10/2021
     */
    callAlertPopup(text, subClass, skuCode) {
      this.hideAlertPopup = false;
      this.alertMessageText = text;
      this.subClass = subClass;
      if (text.includes(skuCode)) {
        let arr = text.split(`${skuCode}`);
        skuCode = `<b>${skuCode}</b>`;
        arr.splice(1, 0, skuCode);
        this.alertMessageText = arr.join(" ");
      }
    },

    /**
     * Hàm đóng toastmessage
     * Ngọc 2/8/2021
     */
    closeToastMessage() {
      this.hideToastMessage = true;
      this.toastMessageText = "";
      this.subClass = "";
    },

    /**
     * Hàm đóng popup cảnh báo
     * Created By: Ngọc 04/10/2021
     */
    closeAlertPopup() {
      try {
        this.hideAlertPopup = true;
        this.alertMessageText = "";
        this.subClass = "";
      } catch (err) {
        console.log(err);
      }
    },
  },

  created() {
    eventBus.$on("closeAlertPopup", () => {
      this.closeAlertPopup();
    });
  },

  watch: {},

  mounted() {},
};
</script>

<style></style>
