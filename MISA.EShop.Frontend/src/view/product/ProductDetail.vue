<template>
  <div
    id="formDetail"
    :productId="productId"
    :formMode="formMode"
    Item="ProductCode"
    :class="['z-index-2', { 'd-none': dnone }]"
  >
    <div class="save-or-cancel">
      <ButtonIcon
        iconName="icon-save"
        buttonText="Lưu"
        subClass="save"
        @btn-click="btnSaveOnClick"
      />
      <ButtonIcon
        iconName="icon-cancel"
        buttonText="Hủy bỏ"
        subClass="cancel"
        @btn-click="btnDialogCancelOnClick"
      />
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

        <Picture labelText="Ảnh hàng hóa" />
      </div>
    </div>

    <div class="save-or-cancel">
      <ButtonIcon
        iconName="icon-save"
        buttonText="Lưu"
        subClass="save"
        @btn-click="btnSaveOnClick"
      />
      <ButtonIcon
        iconName="icon-cancel"
        buttonText="Hủy bỏ"
        subClass="cancel"
        @btn-click="btnDialogCancelOnClick"
      />
    </div>

    <Popup
      :dnone="hidePopup"
      :property="property"
      @closePopupInfo="closePopupInfo"
      @getIdForCombobox="getIdForCombobox"
    />
    <!-- của loader chỉ làm mờ bên trong form detail -->
    <!-- <div :class="['pageCover z-index-1', { 'd-none': HideLoader }]"></div> -->
  </div>
</template>

<script>
import axios from "axios";
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
import Popup from "../../components/base/BasePopup.vue";

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
    Popup,
  },

  props: {
    dnone: {
      type: Boolean,
      default: true,
      require: true,
    },
    productId: {
      type: String,
      default: "",
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
      colors: [],
      color: "",

      //---------------Popup thêm thông tin-----------------//
      hidePopup: true,
      property: "",

      defaultName: "",
      initValue: "",
      reFocus: false,
      check: true,
      // list chứa thuộc tính(màu sắc) sản phẩm
    };
  },

  watch: {
    formMode: function () {
      let me = this;
      if (me.formMode == 0) {
        me.resetFormDetail();
        this.colors = [];
        this.products = [];
        this.productId = "";
        this.productDefault = [];
      }
      if (me.formMode == 1) {
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
    },
    dnone: function () {
      this.reFocus = !this.reFocus;
    },
  },

  methods: {
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
        this.closePopupInfo();
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm đóng form thêm thông tin đơn vị, nhóm hh
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
    addInfomation(hidePopup, fieldName) {
      this.hidePopup = hidePopup;
      this.property = fieldName;
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

      axios
        .post(`${Constant.LocalUrl}/Products`, this.product)
        .then(() => {
          me.$emit(
            "callToastMessage",
            "Thêm dữ liệu thành công",
            "message-green"
          );
          me.$emit("resetAfterSaveData");
        })
        .catch((err) => {
          console.log(err);
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
      axios
        .put(`${Constant.LocalUrl}/Products/${me.productId}`, me.product)
        .then(() => {
          me.$emit(
            "callToastMessage",
            "Sửa dữ liệu thành công",
            "message-green"
          );
          me.$emit("resetAfterSaveData");
        })
        .catch((err) => {
          console.log(err);
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
        productDetail.ProductName = `${this.product.ProductName}/${color}`;
        if (this.productId != "") {
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
      this.product = newEntity;
      // gọi hàm lấy mã hàng hóa mới
      //this.getNewCode();
      // bỏ các tooltip cảnh báo nếu đang lỗi
      this.removeError();
    },

    /**
     * Hàm loại bỏ các tooltip cảnh báo
     * Ngọc 10/8/2021
     */
    removeError() {
      let me = this;
      for (let [key] of Object.entries(me.$refs)) {
        if (key.includes("txt")) me.$refs[key].removeError();
      }
    },

    /**
     * Hàm đóng formDetail
     * Ngọc 29/07/2021
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
          me.addProduct();
        } else if (me.formMode == Enumeration.FormMode.Edit) {
          me.editProduct();
        } else {
          me.duplicateProduct();
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

      for (let [key] of Object.entries(me.$refs)) {
        if (key.includes("required")) {
          let valRes = me.$refs[key].isRequired();

          if (valRes == false) {
            me.$emit(
              "callToastMessage",
              "Không được để trống các trường bắt buộc",
              "message-red"
            );
            isValid = false;
          }
        }
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
};
</script>

<style scoped>
</style>