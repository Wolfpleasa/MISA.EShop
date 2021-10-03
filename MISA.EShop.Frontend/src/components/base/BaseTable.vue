<template>
  <div class="grid">
    <div class="gridEntity">
      <table>
        <Dropdown
          :itemName="itemName"
          :left="left"
          :width="width"
          @chooseDropdownItem="chooseDropdownItem"
        />
        <thead>
          <tr>
            <th><CheckBox /></th>
            <th>
              <div class="th-text">Mã SKU</div>
              <div class="d-flex">
                <Finding
                  @operatorOnClick="operatorOnClick"
                  filterField="SKUCode"
                  :inputOperator="
                    filterParam.SKUCode ? filterParam.SKUCode.value : ''
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.SKUCode.valueFinding"
                />
              </div>
            </th>
            <th>
              <div class="th-text">Tên hàng hóa</div>
              <div class="d-flex">
                <Finding
                  @operatorOnClick="operatorOnClick"
                  filterField="ProductName"
                  :inputOperator="
                    filterParam.ProductName
                      ? filterParam.ProductName.value
                      : '*'
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.ProductName.valueFinding"
                />
              </div>
            </th>
            <th>
              <div class="th-text">Nhóm hàng hóa</div>
              <div class="d-flex">
                <Finding
                  @operatorOnClick="operatorOnClick"
                  filterField="ProductGroupName"
                  :inputOperator="
                    filterParam.ProductGroupName
                      ? filterParam.ProductGroupName.value
                      : '*'
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.ProductGroupName.valueFinding"
                />
              </div>
            </th>
            <th>
              <div class="th-text">Đơn vị tính</div>
              <div class="d-flex">
                <Finding
                  @operatorOnClick="operatorOnClick"
                  filterField="UnitName"
                  :inputOperator="
                    filterParam.UnitName ? filterParam.UnitName.value : '*'
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.UnitName.valueFinding"
                />
              </div>
            </th>
            <th>
              <div class="th-text">Giá bán TB</div>
              <div class="d-flex">
                <FilterPrice
                  @operatorOnClick="operatorOnClick"
                  filterField="SellingPrice"
                  :inputOperator="
                    filterParam.SellingPrice
                      ? filterParam.SellingPrice.value
                      : ''
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.SellingPrice.valueFinding"
                />
              </div>
            </th>
            <th>
              <div class="th-text">Hiển thị trên MH bán hàng</div>
              <div>
                <FilterOption
                  @selectOnClick="selectOnClick"
                  filterField="DisplayOption"
                  :inputValue="
                    filterParam.DisplayOption
                      ? filterParam.DisplayOption.value
                      : ''
                  "
                />
              </div>
            </th>
            <th>
              <div class="th-text">Loại hàng hóa</div>
              <FilterOption
                filterField="Category"
                :inputValue="
                  filterParam.Category ? filterParam.Category.value : ''
                "
              />
            </th>
            <th>
              <div class="th-text">Quản lí theo</div>
              <FilterOption
                filterField="Manage"
                :inputValue="filterParam.Manage ? filterParam.Manage.value : ''"
              />
            </th>
            <th>
              <div class="th-text">Trạng thái</div>
              <FilterOption
                @selectOnClick="selectOnClick"
                filterField="StatusBusinessOption"
                :inputValue="
                  filterParam.StatusBusinessOption
                    ? filterParam.StatusBusinessOption.value
                    : ''
                "
              />
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="(product, index) in products"
            :key="index"
            @click="trOnClick(index)"
            :class="{ 'tr-select': isSelected[index] }"
          >
            <td><CheckBox /></td>
            <td>{{ product.SKUCode }}</td>
            <td>{{ product.ProductName }}</td>
            <td>{{ product.ProductGroupName }}</td>
            <td>{{ product.UnitName }}</td>
            <td>{{ product.SellingPrice }}</td>
            <td>{{ product.Display }}</td>
            <td>Hàng hóa</td>
            <td>Khác</td>
            <td>{{ product.StatusBusiness }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <PageNavigation />
  </div>
</template>

<script>
import axios from "axios";
import Constant from "../../common/constant1.js";
import CommonFn from "../../common/common1.js";

import Finding from "./BaseFinding.vue";
import FilterPrice from "./BaseFilterPrice.vue";
import FilterOption from "./BaseFilterOption.vue";
import CheckBox from "./BaseCheckBox.vue";
import Dropdown from "./BaseDropdown.vue";
import PageNavigation from "./BasePageNavigation.vue";
export default {
  name: "BaseTable",
  components: {
    Finding,
    CheckBox,
    Dropdown,
    FilterPrice,
    FilterOption,
    PageNavigation,
  },

  props: {
    response: String,
  },

  data() {
    return {
      items: [],
      left: 0,
      width: 0,
      itemName: "",
      itemId: "",
      filterParam: {},
      // Danh sách hàng hóa
      products: [],
      // Danh sách nhóm hàng hóa
      productGroups: [],
      // Danh sách đơn vị tính
      units: [],
      // Danh sách tr được chọn
      isSelected: [],
    };
  },

  methods: {
    /**
     * Hàm gửi id lên trên để các toolbar thực hiện
     * Created By: Ngọc 28/09/2021
     */
    getId() {
      let me = this;
      me.products.forEach((product, index) => {
        if (me.isSelected[index]) {
          this.$emit('getId', product.ProductId, product.ParentId);
        }
      });
    },

    /**
     * Hàm chọn 1 hàng
     * Created By: Ngọc 28/09/2021
     */
    trOnClick(index) {
      try {
        let me = this;
        for (var i = 0; i < me.products.length; i++) {
          me.$set(me.isSelected, i, false);
        }
        me.$set(me.isSelected, index, true);
        me.getId();
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm bắt sự kiện chọn giá trị từ component con gửi lên
     * Created By: Ngọc 26/09/2021
     */
    operatorOnClick(itemName, filterField, left) {
      try {
        this.itemName = itemName;
        if (this.left == left) {
          this.left = 0;
        } else {
          this.left = left;
        }
        this.currentFilterField = filterField;
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm bắt sự kiện chọn giá trị từ component con gửi lên
     * Ngọc 26/09/2021
     */
    selectOnClick(filterField, left, width) {
      this.itemName = filterField;
      this.width = width;
      if (this.left == left) {
        this.left = 0;
      } else {
        this.left = left;
      }
      this.currentFilterField = filterField;
    },

    /**
     * Hàm bắt sự kiện bấm chọn 1 giá trị từ component con gửi lên
     * Ngọc 26/09/2021
     */
    chooseDropdownItem(left, value) {
      this.left = left;
      this.filterParam[this.currentFilterField].value = value;
    },

    /**
     * Hàm lấy dữ liệu cho table
     * Ngọc 30/07/2021
     */
    loadDataTable() {
      let me = this;
      me.products = [];
      let url = `${Constant.LocalUrl}/Products`;
      axios
        .get(url)
        .then((res) => {
          if (res.status == 200) {
            me.products = res.data;
            // me.totalEntity = res.data.TotalRecord;
            // me.totalPageNumber = res.data.TotalPageNumber;
            // me.realEntityPerPage = res.data.Entities.length;
            // format các product
            me.format(me.products);
            me.resetTr();
          } else if (res.status == 204) {
            // me.totalEntity = 0;
            // me.totalPageNumber = 1;
            // me.currentPageNumber = 1;
            me.products = [];
          }
        })
        .catch((res) => {
          console.log(res);
        });
    },

    /**
     * Hàm reset các tr về không được chọn rồi chọn tr đầu
     * Created By: Ngọc 28/09/2021
     */
    resetTr() {
      try {
        let me = this;
        me.isSelected = [];
        me.products.forEach(() => {
          me.isSelected.push(false);
        });
        me.$set(me.isSelected, 0, true);
        me.getId();
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm format sau khi lấy dữ liệu
     * Created By: Ngọc 27/09/2021
     */
    format(products) {
      let me = this;
      products.forEach(function (product) {
        if (product["SellingPrice"]) {
          product["SellingPrice"] = CommonFn.formatMoney(
            product["SellingPrice"]
          );
        }

        if (product["Display"] != null) {
          product["Display"] = CommonFn.getValueEnum(
            product["Display"],
            "Display"
          );
        }

        if (product["StatusBusiness"] != null) {
          product["StatusBusiness"] = CommonFn.getValueEnum(
            product["StatusBusiness"],
            "StatusBusiness"
          );
        }

        me.getProductGroupName(product);
        me.getUnitName(product);
      });
    },

    /**
     * Hàm render tên phòng ban
     * Created By: Ngọc 27/09/2021
     */
    getProductGroupName(product) {
      let me = this;
      me.productGroups.forEach(function (productGroup) {
        if (product.ProductGroupId == productGroup.ProductGroupId) {
          product.ProductGroupName = productGroup.ProductGroupName;
        }
      });
    },

    /**
     * Hàm render tên vị trí
     * Created By: Ngọc 27/09/2021
     */
    getUnitName(product) {
      let me = this;
      me.units.forEach(function (unit) {
        if (product.UnitId == unit.UnitId) {
          product.UnitName = unit.UnitName;
        }
      });
    },

    /**
     * load dữ liệu nhóm hàng hóa
     * Created By: Ngọc 25/09/2021
     */
    getProductGroup() {
      let me = this;
      axios
        .get(`${Constant.LocalUrl}/ProductGroups`)
        .then((res) => {
          me.productGroups = res.data;
        })
        .catch((res) => {
          me.callToastMessage(res, "message-red");
        });
    },

    /**
     * load dữ liệu đơn vị tính
     * Created By: Ngọc 25/09/2021
     */
    getUnit() {
      let me = this;
      axios
        .get(`${Constant.LocalUrl}/Units`)
        .then((res) => {
          me.units = res.data;
        })
        .catch((res) => {
          me.callToastMessage(res, "message-red");
        });
    },
  },
  created() {
    this.filterParam = {
      SKUCode: {},
      ProductName: {},
      ProductGroupName: {},
      UnitName: {},
      SellingPrice: {},
      DisplayOption: {},
      Category: {},
      Manage: {},
      StatusBusinessOption: {},
    };

    this.getUnit();
    this.getProductGroup();
    this.loadDataTable();
  },

  watch: {
    response: function(){
      this.loadDataTable();
    }
  },
};
</script>

<style scoped>
</style>
