<template>
  <div :class="['popup z-index-12', { 'd-none': dnone }]">
    <div class="head">
      <div class="head-text">Thêm mới {{ property }}</div>
      <div @click="btnCancelOnClick" class="head-close"></div>
    </div>
    <div class="middle">
      <div v-if="property == 'Unit'">
        <Input
          labelText="Đơn vị tính"
          tabindex="0"
          type="text"
          FieldName="UnitName"
          maxlength="100"
          ref="txtRequiredUnitName"
          obligate="true"
          :reFocus="reFocus"
          autoFocus="true"
          v-model="unit.UnitName"
          :initValue="unit.UnitName"
        />

        <Input
          labelText="Diễn giải"
          tabindex="0"
          type="text"
          FieldName="Description"
          maxlength="255"
          subClass="description"
          ref="txtDescription"
          v-model="unit.Description"
          :initValue="unit.Description"
        />
      </div>

      <div v-else-if="property == 'ProductGroup'">
        <Input
          labelText="Mã nhóm hàng hóa"
          tabindex="0"
          type="text"
          FieldName="ProductGroupCode"
          maxlength="20"
          ref="txtProductGroupCode"
          :reFocus="reFocus"
          autoFocus="true"
          obligate="true"
          v-model="productGroup.ProductGroupCode"
          :initValue="productGroup.ProductGroupCode"
        />

        <Input
          labelText="Tên nhóm hàng hóa"
          tabindex="0"
          type="text"
          FieldName="ProductGroupName"
          maxlength="100"
          ref="txtRequiredProductGroupName"
          obligate="true"
          v-model="productGroup.ProductGroupName"
          :initValue="productGroup.ProductGroupName"
        />

        <Input
          labelText="Mô tả"
          tabindex="0"
          type="text"
          FieldName="Description"
          maxlength="255"
          subClass="description"
          ref="txtDescription"
          v-model="productGroup.Description"
          :initValue="productGroup.Description"
        />
      </div>
    </div>
    <div class="foot">
      <ButtonIcon
        iconName="icon-cancel"
        buttonText="Hủy bỏ"
        subClass="cancel"
        @btn-click="btnCancelOnClick"
      />
      <ButtonIcon
        iconName="icon-save"
        buttonText="Lưu"
        subClass="confirm"
        @btn-click="btnConfirmOnClick"
      />
    </div>
  </div>
</template>

<script>
import axios from "axios";
import Constant from "../../common/constant1.js";

import ButtonIcon from "../base/BaseButtonIcon.vue";
import Input from "../base/BaseInput.vue";
export default {
  name: "BasePopup",
  components: {
    ButtonIcon,
    Input,
  },

  props: {
    dnone: {
      type: Boolean,
      default: true,
      require: true,
    },
    property: String,
    subClass: String,
  },

  data() {
    return {
      unit: {},
      productGroup: {},
      reFocus: false,
    };
  },
  methods: {
    /**
     * Hàm đóng popup
     * Created By: Ngọc 01/10/2021
     */
    btnCancelOnClick() {
      let me = this;
      me.$emit("closePopupInfo");
    },

    /**
     * Hàm bấm nút xác nhận
     * Created By: Ngọc 01/10/2021
     */
    btnConfirmOnClick() {
      let me = this,
        models = "",
        model = {};
      if (me.property == "Unit") {
        models = "Units";
        model = { ...me.unit };
      } else if (this.property == "ProductGroup") {
        models = "ProductGroups";
        model = { ...me.productGroup };
      }

      axios
        .post(`${Constant.LocalUrl}/${models}`, model)
        .then(() => {
          me.getIdForCombobox(models,model);
        })
        .catch((err) => {
          console.log(err);
        });
    },

    /**
     * Hàm lấy lại id vừa thêm để hiện giá trị combobox
     * Created By: Ngọc 01/10/2021
     */
    getIdForCombobox(models, model){
      let me = this,
        itemName = "",
        itemId = ""
      if (me.property == "Unit") {
        itemName = "UnitName";
         itemId = "UnitId" ;
      } else if (this.property == "ProductGroup") {       
        itemName = "ProductGroupName";
        itemId = "ProductGroupId";
        }
      axios.get(`${Constant.LocalUrl}/${models}/${model[itemName]}`)
            .then((res)=>{
              console.log(res);
               console.log( res.data[0][itemId], res.data[0][itemName]);
              this.$emit("getIdForCombobox", res.data[0][itemId], res.data[0][itemName]);
            }).catch(err => {
              console.log(err);
            })
    }
  },

  watch: {
    dnone: function () {
      this.reFocus = !this.reFocus;
    },
  },
};
</script>

<style scoped></style>
