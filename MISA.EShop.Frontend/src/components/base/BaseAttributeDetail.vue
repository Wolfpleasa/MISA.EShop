<template>
  <div class="eshop-attrDetail" v-if="productDetails && productDetails.length > 0">
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
              <input
                @input="onInput($event.target.value, 'PurchasePrice', index)"
                class="ta-r"
                v-model="product.PurchasePrice"
              />
            </td>
            <td>
              <input
                @input="onInput($event.target.value, 'SellingPrice', index)"
                class="ta-r"
                v-model="product.SellingPrice"
              />
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
import CommonFn from "../../common/common1.js";

export default {
  name: "BaseAttributeDetail",
  components: {},

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
    };
  },

  methods: {
    /**
     * Hàm format các giá trị nhập
     * Created By: Ngọc 29/09/2021
     */
    onInput(inputValue, FieldName, index) {
      let me = this;
      let val = CommonFn.formatNumber(inputValue);
      me.productDetails[index][FieldName] = CommonFn.formatMoney(val);
    },

    /**
     * Hàm xóa hàng hóa chi tiết khỏi bảng
     * Created By: Ngọc 24/09/2021
     */
    removeProductDetail(product) {
      this.$emit("deleteAttribute", product.Color);
    },
  },
  watch: {
    products: function(value) {
      this.productDetails = value;
      // format các ô tiền
      if (this.productDetails) {
        this.productDetails.forEach((product) => {
          if (product.PurchasePrice) {
            product.PurchasePrice = CommonFn.formatMoney(product.PurchasePrice);
          }
          if (product.SellingPrice) {
            product.SellingPrice = CommonFn.formatMoney(product.SellingPrice);
          }
        });
      }
    },

    productName: function() {
      this.productDetails.forEach((product) => {
        product.ProductName = `${this.productName} (${product.Color})`;
      });
    },

    skuCode: function() {
      this.productDetails.forEach((product) => {
        product.SKUCode = `${this.skuCode}-${product.Color.charAt(0)}`;
      });
    },
  },

  mounted() {},
};
</script>

<style scoped></style>
