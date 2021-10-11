<template>
  <div class="grid">
    <div class="gridEntity">
      <table>
        <Dropdown
          :itemName="itemName"
          :left="left"
          :width="width"
          :count="count"
          @chooseDropdownItem="chooseDropdownItem"
          @resetPos="resetPos"
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
                    filterParam.SKUCode ? filterParam.SKUCode.OperatorIndex : ''
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.SKUCode.Value"
                  @input="startFinding"
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
                      ? filterParam.ProductName.OperatorIndex
                      : '*'
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.ProductName.Value"
                  @input="startFinding"
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
                      ? filterParam.ProductGroupName.OperatorIndex
                      : '*'
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.ProductGroupName.Value"
                  @input="startFinding"
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
                    filterParam.UnitName
                      ? filterParam.UnitName.OperatorIndex
                      : '*'
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.UnitName.Value"
                  @input="startFinding"
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
                      ? filterParam.SellingPrice.OperatorIndex
                      : ''
                  "
                />
                <input
                  class="input-filter"
                  type="text"
                  v-model="filterParam.SellingPrice.Value"
                />
              </div>
            </th>
            <th>
              <div class="th-text">Hiển thị trên MH bán hàng</div>
              <div>
                <FilterOption
                  @selectOnClick="selectOnClick"
                  filterField="Display"
                  :inputValue="
                    filterParam.Display ? filterParam.Display.OperatorIndex : ''
                  "
                />
              </div>
            </th>
            <th>
              <div class="th-text">Loại hàng hóa</div>
              <div class="filter-option" ref="myComboBox">
                <input class="inp" value="Tất cả" />
                <div class="select-arrow">
                  <div class="arrow"></div>
                </div>
              </div>
            </th>
            <th>
              <div class="th-text">Quản lí theo</div>
              <div class="filter-option" ref="myComboBox">
                <input class="inp" value="Tất cả" />
                <div class="select-arrow">
                  <div class="arrow"></div>
                </div>
              </div>
            </th>
            <th>
              <div class="th-text">Trạng thái</div>
              <FilterOption
                @selectOnClick="selectOnClick"
                filterField="StatusBusiness"
                :inputValue="
                  filterParam.StatusBusiness
                    ? filterParam.StatusBusiness.OperatorIndex
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
            <td @click="$emit('btnEditOnClick')">{{ product.ProductName }}</td>
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
    <PageNavigation
      :entityPerPage="entityPerPage"
      :realEntityPerPage="realEntityPerPage"
      :totalEntity="totalEntity"
      :currentPageNumber="currentPageNumber"
      :totalPageNumber="totalPageNumber"
      @chooseQuantity="chooseQuantity"
      @updatePage="updatePage"
    />
  </div>
</template>

<script>
import { debounce } from "debounce";
import axios from "axios";
import Constant from "../../common/constant1.js";
import CommonFn from "../../common/common1.js";

import Finding from "./BaseFinding.vue";
import FilterPrice from "./BaseFilterPrice.vue";
import FilterOption from "./BaseFilterOption.vue";
import CheckBox from "./BaseCheckBox.vue";
import Dropdown from "./BaseDropdown.vue";
import PageNavigation from "./BasePageNavigation.vue";
import Enumeration from "../../common/enumeration.js";
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
    count: Number,
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

      // -------------------- Phân trang ----------------------
      // Tổng số hàng hóa
      totalEntity: 0,
      // Tổng số trang
      totalPageNumber: 1,
      // Trang hiện tại
      currentPageNumber: 1,
      // Số bản ghi mỗi trang dự kiến
      entityPerPage: 50,
      // Số bản ghi thực tế mỗi trang
      realEntityPerPage: 1,
    };
  },

  methods: {
    /**
     * Hàm thực hiện tìm kiếm theo từ khóa
     * Created By: Ngọc 26/10/2021
     */
    startFinding: debounce(function () {
      this.loadDataTable();
      this.$emit("refreshOnClick");
    }, 500),

    /**
     * Hàm được gọi khi thay đổi page hoặc số lượng hàng hóa/trang
     * Created By: Ngọc 26/10/2021
     */
    updatePage(currentPageNumber) {
      let me = this;
      me.currentPageNumber = currentPageNumber;
      this.loadDataTable();
      me.$emit("refreshOnClick");
    },

    /**
     * Hàm chọn số lượng bản ghi trên 1 trang
     * Created By: Ngọc 27/09/2021
     */
    chooseQuantity(entityPerPage) {
      this.entityPerPage = entityPerPage;
      this.loadDataTable();
      this.$emit("refreshOnClick");
    },

    /**
     * Hàm gửi id lên trên để các toolbar thực hiện
     * Created By: Ngọc 28/09/2021
     */
    getId() {
      let me = this;
      if (me.products.length > 0) {
        me.products.forEach((product, index) => {
          if (me.isSelected[index]) {
            this.$emit("getId", product.ProductId);
          }
        });
      } else {
        this.$emit("getId", null);
      }
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
     * Created By: Ngọc 26/09/2021
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
     *  Created By: Ngọc 26/09/2021
     */
    chooseDropdownItem(left, OperatorIndex) {
      this.left = left;
      this.filterParam[this.currentFilterField].OperatorIndex = OperatorIndex;
      this.loadDataTable();
      this.$emit("refreshOnClick");
    },

    /**
     * Hàm bắt sự kiện bấm ra ngoài hộp chọn
     *  Created By: Ngọc 26/09/2021
     */
    resetPos(left) {
      this.left = left;
    },

    /**
     * Hàm lấy dữ liệu cho table
     * Created By: Ngọc 26/09/2021
     */
    loadDataTable() {
      let me = this;
      me.products = [];
      let filterParams = [];
      for (let key in me.filterParam) {
        let field = me.filterParam[key];
        if (field.Value || key == "Display" || key == "StatusBusiness")
          filterParams.push(field);
      }
      let url = `${Constant.LocalUrl}/Products/paging?pageSize=${me.entityPerPage}&pageNumber=${me.currentPageNumber}`;
      axios
        .post(url, filterParams)
        .then((res) => {
          if (res.status == Enumeration.Status.OK) {
            me.products = res.data.Products;
            me.totalEntity = res.data.TotalRecord;
            me.totalPageNumber = res.data.TotalPageNumber;
            me.realEntityPerPage = res.data.Products.length;
            // format các product
            me.format(me.products);
            me.resetTr();
          } else if (
            res.status == Enumeration.Status.NoContent ||
            res.status == Enumeration.Status.ServerError
          ) {
            me.totalEntity = 0;
            me.realEntityPerPage = 0;
            me.totalPageNumber = 1;
            me.currentPageNumber = 1;
            me.products = [];
            me.getId();
          }
        })
        .catch((res) => {
          console.log(res);
        });
    },

    /**
     * Hàm reset các tr về không được chọn rồi chọn tr đầu
     * Created By: Ngọc 26/09/2021
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
     * Created By: Ngọc 26/09/2021
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
     * Created By: Ngọc 26/09/2021
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
     * Created By: Ngọc 26/09/2021
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
      SKUCode: {
        OperatorIndex: 0,
        Field: "SKUCode",
        Value: null,
      },
      ProductName: { OperatorIndex: 0, Field: "ProductName", Value: null },
      ProductGroupName: {
        OperatorIndex: 0,
        Field: "ProductGroupName",
        Value: null,
      },
      UnitName: { OperatorIndex: 0, Field: "UnitName", Value: null },
      SellingPrice: { OperatorIndex: 0, Field: "SellingPrice", Value: null },
      Display: { OperatorIndex: 2, Field: "Display" },
      Category: { OperatorIndex: 2, Field: "Category" },
      Manage: { OperatorIndex: 2, Field: "StatusBusiness" },
      StatusBusiness: { OperatorIndex: 2, Field: "StatusBusiness" },
    };

    this.getUnit();
    this.getProductGroup();
    this.loadDataTable();
  },

  watch: {
    response: function () {
      this.loadDataTable();
    },
  },
};
</script>

<style scoped></style>
