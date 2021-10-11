<template>
  <div
    id="formDetail"
    :productId="productId"
    :formMode="formMode"
    Item="ProductCode"
    :class="['z-index-2', { 'd-none': dnone }]"
    @keydown="keyDownOnClick($event)"
  >
    <div class="save-or-cancel">
      <div class="p-relative tt-field">
        <ButtonIcon
          iconName="icon-save"
          buttonText="Lưu"
          subClass="save"
          @btn-click="btnSaveOnClick"
        />
        <ToolTip toolTipText="Ctrl + S" :hideToolTip="hideToolTip" />
      </div>
      <div class="p-relative tt-field">
        <ButtonIcon
          iconName="icon-cancel"
          buttonText="Hủy bỏ"
          subClass="cancel"
          @btn-click="btnDialogCancelOnClick"
        />
        <ToolTip toolTipText="Hủy (Esc)" :hideToolTip="hideToolTip" />
      </div>
    </div>

    <div class="main">
      <div class="basic-info">
        <div class="title-info">Thông tin cơ bản</div>

        <Radio
          labelText="Trạng thái kinh doanh"
          tabindex="0"
          FieldName="StatusBusiness"
          itemId="StatusBusiness"
          :selectedId="product.StatusBusiness + ''"
          itemName="StatusBusinessName"
          ref="RadioStatusBusiness"
          @chooseRadioItem="chooseRadioItem"
        />

        <Input
          labelText="Tên hàng hóa"
          tabindex="0"
          type="text"
          FieldName="ProductName"
          maxlength="100"
          ref="txtRequiredProductName"
          obligate="true"
          :reFocus="reFocus"
          autoFocus="true"
          v-model="product.ProductName"
          :initValue="product.ProductName"
          @autoGenSKUCode="autoGenSKUCode"
        />

        <ComboBox
          labelText="Nhóm hàng hóa"
          tabindex="0"
          :defaultName="defaultName"
          placeholder="Nhập để tìm kiếm"
          Url="ProductGroups"
          FieldName="ProductGroup"
          ref="ComboBoxProductGroup"
          :selectedId="product.ProductGroupId + ''"
          itemId="ProductGroupId"
          itemName="ProductGroupName"
          @chooseComboBoxItem="chooseComboBoxItem"
          @addInfomation="addInfomation"
        />

        <Input
          labelText="Mã SKU"
          tabindex="0"
          type="text"
          placeholder="Hệ thống sinh tự động khi bỏ trống"
          FieldName="SKUCode"
          maxlength="20"
          ref="txtSKUCode"
          v-model="product.SKUCode"
          :initValue="product.SKUCode"
        />

        <Input
          labelText="Giá mua"
          tabindex="0"
          type="text"
          FieldName="PurchasePrice"
          maxlength="20"
          ref="txtPurchasePrice"
          subClass="price"
          v-model="product.PurchasePrice"
          :initValue="product.PurchasePrice"
          @convertMoney="convertMoney"
        />

        <Input
          labelText="Giá bán"
          tabindex="0"
          type="text"
          FieldName="SellingPrice"
          maxlength="20"
          subClass="price"
          ref="txtSellingPrice"
          v-model="product.SellingPrice"
          :initValue="product.SellingPrice"
          @convertMoney="convertMoney"
        />

        <ComboBox
          labelText="Đơn vị tính"
          tabindex="0"
          :defaultName="defaultName"
          placeholder="Nhập để tìm kiếm"
          Url="Units"
          FieldName="Unit"
          ref="ComboBoxUnit"
          :selectedId="product.UnitId + ''"
          itemId="UnitId"
          itemName="UnitName"
          @chooseComboBoxItem="chooseComboBoxItem"
          @addInfomation="addInfomation"
        />
      </div>
      <div class="pd-l-10 d-flex">
        <CheckBox :checked="check" @clickCheckBox="clickCheckBox" />
        <div class="ml-5">Hiện thị trên màn hình bán hàng</div>
        <div class="quiz"></div>
      </div>
      <div class="attribute-info">
        <div class="title-info">Thông tin thuộc tính</div>

        <Attribute
          labelText="Thuộc tính"
          attributeName="Màu sắc"
          tabindex="0"
          type="text"
          placeholder="Xanh, Đỏ, Vàng..."
          FieldName="Color"
          maxlength="100"
          ref="attrColor"
          v-model="colors"
          @delete="attrColorOnDelete($event)"
          @addColorToTableDetail="addColorToTableDetail($event)"
        />

        <AttributeDetail
          labelText="Chi tiết thuộc tính"
          :products="product.ProductDetails"
          :productName="product.ProductName"
          :skuCode="product.SKUCode"
          @deleteAttribute="deleteAttribute"
        />
      </div>
      <div class="additional-info">
        <div class="title-info">Thông tin bổ sung</div>

        <Input
          labelText="Mô tả"
          tabindex="0"
          type="text"
          FieldName="Description"
          maxlength="255"
          subClass="description"
          ref="txtDescription"
          v-model="product.Description"
          :initValue="product.Description"
        />

        <Picture
          labelText="Ảnh hàng hóa"
          @chooseFile="chooseFile"
          :pictureId="product.PictureId"
        />
      </div>
    </div>

      <div class="save-or-cancel">
      <div class="p-relative tt-field">
        <ButtonIcon
          iconName="icon-save"
          buttonText="Lưu"
          subClass="save"
          @btn-click="btnSaveOnClick"
        />
        <ToolTip toolTipText="Ctrl + S" :hideToolTip="hideToolTip" />
      </div>
      <div class="p-relative tt-field">
        <ButtonIcon
          iconName="icon-cancel"
          buttonText="Hủy bỏ"
          subClass="cancel"
          @btn-click="btnDialogCancelOnClick"
        />
        <ToolTip toolTipText="Hủy (Esc)" :hideToolTip="hideToolTip" />
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { eventBus } from "../../main.js";

import CommonFn from "../../common/common1.js";
import Constant from "../../common/constant1.js";
import Enumeration from "../../common/enumeration.js";

import ButtonIcon from "../../components/base/BaseButtonIcon.vue";
import Radio from "../../components/base/BaseRadio.vue";
import Input from "../../components/base/BaseInput.vue";
import ComboBox from "../../components/base/BaseComboBox1.vue";
import CheckBox from "../../components/base/BaseCheckBox.vue";
import Attribute from "../../components/base/BaseAttribute.vue";
import AttributeDetail from "../../components/base/BaseAttributeDetail.vue";
import Picture from "../../components/base/BasePicture.vue";
import ToolTip from "../../components/base/BaseToolTip1.vue";
import ResourceVN from "../../common/resourceVN.js";

export default {
  name: "ProductDetail",
  components: {
    ButtonIcon,
    Radio,
    Input,
    ComboBox,
    CheckBox,
    Attribute,
    AttributeDetail,
    Picture,
    ToolTip,
  },

  props: {
    dnone: {
      type: Boolean,
      default: true,
      require: true,
    },
    productId: {
      type: String,
      default: null,
      require: true,
    },
    formMode: Number,
  },

  data() {
    return {
      product: {},

      //-----------Thuộc tính(màu sắc)-----------------//
      products: [],
      productDefault: [],
      productName: "",
      skuCode: "",
      colors: [],
      color: "",

      //---------------Popup thêm thông tin-----------------//
      hidePopup: true,
      property: "",
      defaultName: "",
      initValue: "",
      reFocus: false,
      check: true,

      // Picture
      formFile: null,

      //sự kiên thao tác bằng tổ hợp phím
      pressCtrl: false,
      pressShift: false,
      pressKeyS: false,

      //tooltip
      hideToolTip: true,
    };
  },

  watch: {
    formMode: function () {
      let me = this;
      if (me.formMode == Enumeration.FormMode.Add) {
        me.resetFormDetail();
        this.colors = [];
        this.products = [];
        this.productDefault = [];
      }
      if (me.formMode == Enumeration.FormMode.Edit) {
        me.resetFormDetail();
        axios
          .get(`${Constant.LocalUrl}/Products/detail/${me.productId}`)
          .then((res) => {
            this.product = res.data;
            this.productDefault = [...this.product.ProductDetails];
            this.bindingData();
          })
          .catch((err) => {
            console.log(err);
          });
      }
      if (me.formMode == Enumeration.FormMode.Duplicate) {
        me.resetFormDetail();
        axios
          .get(`${Constant.LocalUrl}/Products/detail/${me.productId}`)
          .then((res) => {
            me.product = res.data;
            me.productDefault = [...me.product.ProductDetails];
            me.bindingData();
            //me.product.SKUCode = ;
            console.log(me.product.SKUCode);
            me.$set(
              me.product,
              "SKUCode",
              CommonFn.increaseSKUCode(
                me.product.ProductName,
                me.product.SKUCode
              )
            );
            console.log(
              CommonFn.increaseSKUCode(
                me.product.ProductName,
                me.product.SKUCode
              )
            );
          })
          .catch((err) => {
            console.log(err);
          });
      }
    },

    dnone: function () {
      this.reFocus = !this.reFocus;
    },
  },

  methods: {
    /**
     * Hàm chọn file ảnh từ component Picture gửi lên
     * Created By: Ngọc 05/10/2021
     */
    chooseFile(file) {
      let me = this;
      me.formFile = file;
      if (file == null) {
        me.product.PictureId = ResourceVN.DEFAULT_PICTUREID;
      } else {
        me.product.PictureId = null;
      }
    },

    /**
     * Hàm lấy mã SKU tự động
     * Created By: Ngọc 04/10/2021
     */
    autoGenSKUCode() {
      let me = this;
      let productName = {};
      productName.ProductName = me.product.ProductName;
      if (me.product.SKUCode == null || me.product.SKUCode == "") {
        axios
          .post(`${Constant.LocalUrl}/Products/getNewCode`, productName)
          .then((res) => {
            this.$set(me.product, "SKUCode", res.data);
          })
          .catch((err) => {
            console.log(err);
          });
      }
    },

    /**
     * Hàm hiển thị tên cho combobox sau khi thêm thuộc tính
     * Created By: Ngọc 01/10/2021
     */
    getIdForCombobox(idValue, name) {
      try {
        let itemId = `${this.property}Id`;
        this.product[itemId] = idValue;
        this.selectedId = this.product[itemId];
        this.defaultName = name;
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm mở và thêm thông tin cơ bản
     * Created By: Ngọc 01/10/2021
     */
    addInfomation(hidePopup, fieldName) {
      this.property = fieldName;
      this.$emit("hidePopupInfo", hidePopup, fieldName);
    },

    /**
     * Hàm binding dữ liệu đã format vào form sửa
     * Created By: Ngọc 30/09/2021
     */
    bindingData() {
      const me = this;
      if (me.product.SellingPrice != null) {
        me.product.SellingPrice = CommonFn.formatMoney(me.product.SellingPrice);
      }

      if (me.product.PurchasePrice != null) {
        me.product.PurchasePrice = CommonFn.formatMoney(
          me.product.PurchasePrice
        );
      }

      if (me.product.Display == Enumeration.Display.Yes) {
        me.check = true;
      } else {
        me.check = false;
      }

      // gán lại vào mảng colors
      let colors = [];
      if (
        me.product &&
        me.product.ProductDetails &&
        me.product.ProductDetails.length
      ) {
        let productDetails = me.product.ProductDetails;
        // build colors from child
        colors = productDetails.map((pds) => pds.Color);
      } else {
        colors = [];
      }
      this.colors = colors;
    },

    /**
     * Thêm sản phẩm mới
     * Created By: Ngọc 28/09/2021
     */
    addProduct() {
      let me = this;
      me.formatData();
      let formData = new FormData();
      formData.append("formFile", me.formFile);
      formData.append("data", JSON.stringify(me.product));
      axios
        .post(`${Constant.LocalUrl}/Products`, formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        })
        .then(() => {
          me.$emit("resetAfterSaveData");
        })
        .catch((error) => {
          console.log(error);
          me.$emit(
            "callAlertPopup",
            `${error.response.data.userMsg}`,
            `${Enumeration.Message.Notify}`,
            `${error.response.data.dataErr}`
          );
        });
    },

    /**
     * Sửa thông tin sản phẩm
     * Created By: Ngọc 28/09/2021
     */
    editProduct() {
      let me = this;
      me.addFlag(me.productDefault, me.product.ProductDetails);
      me.formatData();
      let formData = new FormData();
      formData.append("formFile", me.formFile);
      formData.append("data", JSON.stringify(me.product));
      axios
        .put(`${Constant.LocalUrl}/Products/${me.productId}`, formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        })
        .then(() => {
          me.$emit("resetAfterSaveData");
        })
        .catch((error) => {
          me.$emit(
            "callAlertPopup",
            `${error.response.data.userMsg}`,
            `${Enumeration.Message.Notify}`,
            `${error.response.data.dataErr}`
          );
        });
    },

    /**
     * Hàm đánh dấu các hàng xóa chi tiết để biết đâu là hàng cần sửa,xóa, thêm
     * Created By: Ngọc 01/10/2021
     */
    addFlag(oldArr, newArr) {
      for (let newObj of newArr) {
        if (oldArr.indexOf(newObj) >= 0) {
          newObj.FlagMode = Enumeration.FormMode.Edit;
        } else {
          newObj.FlagMode = Enumeration.FormMode.Add;
        }
      }

      for (let oldObj of oldArr) {
        if (newArr.indexOf(oldObj) < 0) {
          oldObj.FlagMode = Enumeration.FormMode.Delete;
          newArr.push(oldObj);
        }
      }
    },

    /**
     * Hàm format lại các thuộc tính trước khi gọi axios
     * Created By: Ngọc 29/09/2021
     */
    formatData() {
      if (this.product) {
        if (this.product.SellingPrice) {
          this.product.SellingPrice = CommonFn.formatNumber(
            this.product.SellingPrice
          );
        }

        if (this.product.PurchasePrice) {
          this.product.PurchasePrice = CommonFn.formatNumber(
            this.product.PurchasePrice
          );
        }

        if (this.product.ProductGroupId == "")
          this.product.ProductGroupId = null;
        if (this.product.UnitId == "") this.product.UnitId = null;
      }

      if (this.product.ProductDetails) {
        this.product.ProductDetails.forEach((product) => {
          if (product.SellingPrice) {
            product.SellingPrice = CommonFn.formatNumber(product.SellingPrice);
          }

          if (product.PurchasePrice) {
            product.PurchasePrice = CommonFn.formatNumber(
              product.PurchasePrice
            );
          }

          if (product.ProductGroupId == "") product.ProductGroupId = null;
          if (product.UnitId == "") product.UnitId = null;
        });
      } else {
        this.product.ProductDetails = null;
      }
    },

    /**
     * Hàm xóa màu sắc của component con gửi lên
     * Created By: Ngọc 01/10/2021
     */
    attrColorOnDelete(color) {
      this.deleteAttribute(color);
    },

    /**
     * Hàm bắt sự kiện xóa 1 thuộc tính(màu sắc)
     * Created By: Ngọc 24/09/2021
     * Modified By: Ngọc 01/10/2021
     */
    deleteAttribute(color) {
      try {
        if (this.product.ProductDetails && this.colors) {
          // xóa hàng hóa
          const productDetails = this.product.ProductDetails.filter(
            (p) => p.Color !== color
          );
          this.$set(this.product, "ProductDetails", productDetails);
          // xóa màu sắc
          const colors = this.colors.filter((c) => c !== color);
          this.colors = colors;
        }
      } catch (err) {
        console.log(err);
      }
    },

    addColorToTableDetail(color) {
      try {
        // tạo product child
        let productDetail = {};
        productDetail.Color = color;
        let colorArr = color.split(" ");
        colorArr = colorArr.map((c) =>
          CommonFn.removeVietnameseTones(c.charAt(0))
        );
        productDetail.ProductName = `${this.product.ProductName} (${color})`;
        productDetail.SKUCode = `${this.product.SKUCode}-${colorArr.join("")}`;
        if (this.formMode != Enumeration.FormMode.Add) {
          productDetail.ParentId = this.productId;
        }
        // thêm vào mảng product.ProductDetails
        if (this.product.ProductDetails && this.product.ProductDetails.length) {
          this.$set(this.product, "ProductDetails", [
            ...this.product.ProductDetails,
            productDetail,
          ]);
        } else {
          this.$set(this.product, "ProductDetails", [productDetail]);
        }
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm bắt sự kiện thao tác với phím
     * Created By: Ngọc 08/10/2021
     */
    keyDownOnClick(e) {
      try {
        if (e.code == "Escape") {
          e.preventDefault();
          this.btnDialogCancelOnClick();
        }
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm bấm CheckBox
     * Created By: Ngọc 23/09/2021
     */
    clickCheckBox() {
      let me = this;
      me.check = !me.check;
      if (me.check) {
        me.product.Display = Enumeration.Display.Yes;
      } else {
        me.product.Display = Enumeration.Display.No;
      }
    },

    /**
     * Hàm reset form thêm
     * Created By: Ngọc 28/09/2021
     */
    resetFormDetail() {
      // cho object hàng hóa về rỗng
      let newEntity = {};
      newEntity.StatusBusiness = Enumeration.StatusBusiness.Trading;
      newEntity.Display = Enumeration.Display.Yes;
      newEntity.ProductGroupId = "";
      newEntity.UnitId = "";
      newEntity.PictureId = "";
      this.product = newEntity;
      this.formFile = null;
      // gọi hàm lấy mã hàng hóa mới
      //this.getNewCode();
      // bỏ các tooltip cảnh báo nếu đang lỗi
      this.removeError();
    },

    /**
     * Hàm loại bỏ các tooltip cảnh báo
     * Created By: Ngọc 28/09/2021
     */
    removeError() {
      let me = this;
      for (let [key] of Object.entries(me.$refs)) {
        if (key.includes("txt")) me.$refs[key].removeError();
      }
    },

    /**
     * Hàm đóng formDetail
     * Created By: Ngọc 22/09/2021
     */
    btnDialogCancelOnClick() {
      let me = this;
      me.$emit("btnDialogCancelOnClick");
    },

    /**
     * Sự kiện bấm nút lưu
     * Ngọc 29/07/2021
     */
    btnSaveOnClick() {
      let me = this;

      if (me.validateForm()) {
        if (me.formMode == Enumeration.FormMode.Add) {
          if (this.formFile == null) {
            me.product.PictureId = ResourceVN.DEFAULT_PICTUREID;
          }
          me.addProduct();
        } else if (me.formMode == Enumeration.FormMode.Edit) {
          me.editProduct();
        } else if (me.formMode == Enumeration.FormMode.Duplicate) {
          me.addProduct();
        }
      }
    },

    /**
     * Lấy mã hàng hóa mới
     * Ngọc 1/8/2021
     */
    getNewCode() {
      let me = this;
      axios
        .get(`${Constant.LocalUrl}Products/NewProductCode`)
        .then((res) => {
          let newProduct = {};
          newProduct.ProductCode = res.data;
          me.product = newProduct;
        })
        .catch((err) => {
          console.log(err);
        });
    },

    /**
     * Hàm kiển tra dữ liệu
     * Ngọc 2-6-2021
     */
    validateForm() {
      let me = this,
        isValid = true;

      if (isValid) {
        isValid = me.isRequired();
      }

      // if (isValid) {
      //     isValid = me.validateCode(formMode, itemId);
      // }

      return isValid;
    },

    /**
     * Hàm kiểm tra bắt buộc nhập các dữ liệu bắt buộc
     * Ngọc 8/8/2021
     */
    isRequired() {
      let me = this;
      let isValid = true;

      if (me.product.ProductName == "" || me.product.ProductName == null) {
        me.$emit(
          "callAlertPopup",
          "Không được để trống các trường bắt buộc",
          Enumeration.Message.Notify,
          me.product.ProductName
        );

        isValid = false;
      }

      return isValid;
    },

    /**
     * Hàm tự động format tiền
     * Created By: Ngọc 28/09/2021
     */
    convertMoney(FieldName) {
      let me = this;
      let val = me.product[FieldName];
      let val1 = CommonFn.formatNumber(val);
      me.product[FieldName] = CommonFn.formatMoney(val1);
    },

    /**
     * Hàm lấy id của combobox được chọn gán vào obj product
     * Created By: Ngọc 28/09/2021
     */
    chooseComboBoxItem(itemID, itemValue) {
      this.product[itemID] = itemValue;
    },

    /**
     * Hàm lấy id của combobox được chọn gán vào obj product
     * Created By: Ngọc 28/09/2021
     */
    chooseRadioItem(itemID, itemValue) {
      this.product[itemID] = itemValue;
    },
  },

  created() {
    eventBus.$on("getIdForCombobox", (idValue, name) => {
      this.getIdForCombobox(idValue, name);
    });
  },

  mounted() {
    let me = this;
    document.addEventListener("keydown", (e) => {
      switch (e.key) {
        case "Control":
          me.pressCtrl = true;
          break;
        case "s":
          me.pressKeyS = true;
          break;
      }
      if (me.pressCtrl && me.pressKeyS) {
        //document.querySelector("#btn").click();
        me.btnSaveOnClick();
        e.preventDefault();
      }
    });

    document.addEventListener("keyup", (e) => {
      switch (e.key) {
        case "Control":
          me.pressCtrl = false;
          break;
        case "s":
          me.pressKeyS = false;
          break;
      }
    });
  },
};
</script>

<style scoped>
</style>