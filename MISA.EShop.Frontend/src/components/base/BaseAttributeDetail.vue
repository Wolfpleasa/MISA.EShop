<template>
  <div
    class="eshop-attrDetail"
    v-if="productDetails && productDetails.length > 0"
  >
    <div>
      {{ labelText }}
    </div>

    <div class="attrDetail">
      <table>
        <thead>
          <tr>
            <th>Tên hàng hóa</th>
            <th>Mã SKU</th>
            <th>Mã vạch</th>
            <th>Giá mua</th>
            <th>Giá bán</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(product, index) in productDetails" :key="index">
            <td><input v-model="product.ProductName" /></td>
            <td><input v-model="product.SKUCode" /></td>
            <td><input v-model="product.ProductCode" /></td>
            <td>
              <money
                v-bind="money"
                class="ta-r"
                v-model="product.PurchasePrice"
              ></money>
            </td>
            <td>
              <money
                v-bind="money"
                class="ta-r"
                v-model="product.SellingPrice"
              ></money>
            </td>
            <td>
              <div
                class="remove-product"
                @click="removeProductDetail(product)"
              ></div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { Money } from "v-money";

import CommonFn from "../../common/common1.js";

export default {
  name: "BaseAttributeDetail",
  components: { Money },

  props: {
    labelText: String,
    FieldName: String,
    subClass: String,

    products: {
      type: Array,
      default: () => [],
    },

    productName: String,
    skuCode: String,
  },
  data() {
    return {
      // Giá trị/Nội dung hiện tại
      inputValue: "",
      // List các hàng hóa
      productDetails: this.products,
      money: {
        thousands: ".",
        precision: 0,
        masked: false,
      },
    };
  },

  methods: {
    /**
     * Hàm format các giá trị nhập
     * Created By: Ngọc 29/09/2021
     */
    // onInput(inputValue, FieldName, index) {
    //   try {
    //     let me = this;
    //     let val = inputValue + "",
    //       number = "";
    //     for (let i = 0; i < val.length; i++) {
    //       if (!isNaN(val[i])) number += val[i];
    //     }

    //     me.productDetails[index][FieldName] = CommonFn.convertMoney(
    //       number + ""
    //     );
    //   } catch (error) {
    //     console.log(error);
    //   }
    // },

    /**
     * Hàm xóa hàng hóa chi tiết khỏi bảng
     * Created By: Ngọc 24/09/2021
     */
    removeProductDetail(product) {
      try {
        this.$emit("deleteAttribute", product.Color);
      } catch (error) {
        console.log(error);
      }
    },
  },
  watch: {
    products: function (value) {
      this.productDetails = value;
    },

    productName: function () {
      this.productDetails.forEach((product) => {
        product.ProductName = `${this.productName} (${product.Color})`;
      });
    },

    skuCode: function () {
      this.products.forEach((product) => {
        product.SKUCode = `${this.skuCode}-${CommonFn.removeVietnameseTones(
          CommonFn.genSKUCodeDetail(product.Color)
        )}`;
        product.ProductCode = `${this.skuCode}-${
          product.ProductCode.split("-")[1]
        }`;
      });
    },
  },

  mounted() {},
};
</script>

<style scoped></style>
