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
            <th ref="thFirstChild">
              <CheckBox />
            </th>
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
                  @change="startFinding"
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
                  @change="startFinding"
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
                  @change="startFinding"
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
                  @change="startFinding"
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
                  class="input-filter ta-r"
                  type="text"
                  v-model="filterParam.SellingPrice.Value"
                  @change="startFinding"
                  @input="onInput($event.target.value)"
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
            <td>
              <CheckBox
                @clickCheckBox="clickCheckBoxTd"
                :checked="isSelected[index]"
              />
            </td>
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
    refresh: Number,
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
     * Hàm không cho nhập chữ cái a-z,A-Z
     */
    isNumberKey(e) {
      alert(e.which);
      var charCode = e.which ? e.which : e.keyCode;
      if (charCode > 31 && (charCode < 48 || charCode > 57)) return false;
      return true;
    },

    /**
     * Hàm bắt sự kiện nhập để format
     * Created By: Ngọc 26/10/2021
     */
    onInput(inpValue) {
      try {
        let val = inpValue + "",
          number = "";
        for (let i = 0; i < val.length; i++) {
          if (!isNaN(val[i])) number += val[i];
        }

        this.filterParam.SellingPrice.Value = number
          ? parseInt(number) + ""
          : "";

        if (this.filterParam.SellingPrice.Value) {
          this.filterParam.SellingPrice.Value = CommonFn.convertMoney(
            this.filterParam.SellingPrice.Value
          );
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm thực hiện tìm kiếm theo từ khóa
     * Created By: Ngọc 26/10/2021
     */
    startFinding: debounce(function () {
      try {
        this.loadDataTable();
        this.$emit("callLoader");
      } catch (error) {
        console.log(error);
      }
    }, 50),

    /**
     * Hàm được gọi khi thay đổi page hoặc số lượng hàng hóa/trang
     * Created By: Ngọc 26/10/2021
     */
    updatePage(currentPageNumber) {
      let me = this;
      me.currentPageNumber = currentPageNumber;
      this.loadDataTable();
      me.$emit("callLoader");
    },

    /**
     * Hàm chọn số lượng bản ghi trên 1 trang
     * Created By: Ngọc 27/09/2021
     */
    chooseQuantity(entityPerPage, currentPageNumber) {
      try {
        this.entityPerPage = entityPerPage;
        this.currentPageNumber = currentPageNumber;
        this.loadDataTable();
        this.$emit("callLoader");
      } catch (error) {
        console.log(error);
      }
    },

    /**
     * Hàm gửi id lên trên để các toolbar thực hiện
     * Created By: Ngọc 28/09/2021
     */
    getId() {
      try {
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
      } catch (error) {
        console.log(error);
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
        let thLeft = this.$refs.thFirstChild.getBoundingClientRect().left;
        this.itemName = itemName;
        if (this.left == left) {
          this.left = 0;
        } else {
          this.left = left - thLeft;
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
      try {
        let thLeft = this.$refs.thFirstChild.getBoundingClientRect().left;
        this.itemName = filterField;
        this.width = width;
        if (this.left == left) {
          this.left = 0;
        } else {
          this.left = left - thLeft;
        }
        this.currentFilterField = filterField;
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm bắt sự kiện bấm chọn 1 giá trị từ component con gửi lên
     *  Created By: Ngọc 26/09/2021
     */
    chooseDropdownItem(left, OperatorIndex) {
      try {
        this.left = left;
        this.filterParam[this.currentFilterField].OperatorIndex = OperatorIndex;
        this.loadDataTable();
        this.$emit("callLoader");
      } catch (error) {
        console.log(error);
      }
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
        let field = Object.assign({}, me.filterParam[key]);
        if (field.Value || key == "Display" || key == "StatusBusiness")
          filterParams.push(field);
      }

      filterParams.forEach((item) => {
        if (item.Field == "SellingPrice" && item.Value) {
          item.Value = CommonFn.formatNumber(item.Value);
        }
      });

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
     * Hàm bấm checkbox ở td được BaseCheckBox gửi lên
     * Created By: Ngọc 26/09/2021
     */
    clickCheckBoxTd(index) {
      let me = this;
      me.$set(me.isSelected, index, !me.isSelected[index]);
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

    defaultfilterParam() {
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
    },
  },
  created() {
    this.defaultfilterParam();
    this.getUnit();
    this.getProductGroup();
  },

  watch: {
    response: function () {
      this.loadDataTable();
    },

    refresh: function(){
      this.defaultfilterParam();
      this.loadDataTable();
    }
  },
};
</script>

<style scoped></style>
