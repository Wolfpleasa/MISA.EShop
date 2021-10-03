<template>
  <div
    :class="['list-dropdown', { 'd-none': hideListDropdown }]"
    :itemId="itemId"
    :itemName="itemName"
    ref="dropdown"
  >
    <div
      class="dropdown-item"
      v-for="(item, index) in items"
      :key="index"
      @click="chooseDropdownItem(index)"
    >
      {{ item[itemName] }}
    </div>
  </div>
</template>

<script>
import { mixin as clickaway } from "vue-clickaway";
import ResourceVN from "../../common/resourceVN.js";

export default {
  mixins: [clickaway],
  name: "BaseDropdown",

  components: {},

  props: {
    itemId: String,
    itemName: String,
    left: Number,
    width: Number,
  },

  data() {
    return {
      hideListDropdown: true,
      items: [],
    };
  },
  methods: {
    /**
     * Hàm gán giá trị cho các item
     * Created By: Ngọc 26/09/2021
     */
    setValue() {
      try {
        switch (this.itemName) {
          case "OperatorName":
            this.items = [
              {
                Operator: 0,
                OperatorName: "* : Chứa",
              },
              {
                Operator: 1,
                OperatorName: "= : Bằng",
              },
              {
                Operator: 2,
                OperatorName: "+ : Bắt đầu bằng",
              },
              {
                Operator: 3,
                OperatorName: "- : Kết thúc bằng",
              },
              {
                Operator: 4,
                OperatorName: "! : Không chứa",
              },
            ];
            break;

          case "FilterPriceName":
            this.items = [
              {
                FilterPrice: 0,
                FilterPriceName: "= : Bằng",
              },
              {
                FilterPrice: 1,
                FilterPriceName: "< : Nhỏ hơn",
              },
              {
                FilterPrice: 2,
                FilterPriceName: `≤ : Nhỏ hơn hoặc bằng`,
              },
              {
                FilterPrice: 3,
                FilterPriceName: "> : Lớn hơn",
              },
              {
                FilterPrice: 4,
                FilterPriceName: "≥ : Lớn hơn hoặc bằng",
              },
            ];
            break;

          case "DisplayOption":
            this.items = [
              {
                Display: 2,
                DisplayOption: "Tất cả",
              },
              {
                Display: 1,
                DisplayOption: "Có",
              },
              {
                Display: 0,
                DisplayOption: `Không`,
              },            
            ];
            break;

          case "StatusBusinessOption":
            this.items = [
              {
                StatusBusiness: 2,
                StatusBusinessOption: ResourceVN.StatusBusiness.All,
              },
              {
                StatusBusiness: 1,
                StatusBusinessOption: ResourceVN.StatusBusiness.Trading,
              },
              {
                StatusBusiness: 0,
                StatusBusinessOption: ResourceVN.StatusBusiness.StopTrading,
              },            
            ];
            break;

          default:
            break;
        }
      } catch (err) {
        console.log(err);
      }
    },

     /**
     * Sự kiện chọn 1 toán tử
     * Created By: Ngọc 26/09/2021
     */
    chooseDropdownItem(index) {
       this.hideListDropdown = true;
      if(this.itemName == "FilterPriceName" || this.itemName == "OperatorName" ){
          this.$emit("chooseDropdownItem" , 0, this.items[index][this.itemName].charAt(0)); 
      }else{
         this.$emit("chooseDropdownItem" , 0, this.items[index][this.itemName]);  
      }
      
    }
  },

  watch: {
    itemName: function () {
      this.setValue();
      if(this.itemName == "FilterPriceName" || this.itemName == "OperatorName" ){
         this.$refs.dropdown.style.width = "auto";     
      }else{
        this.$refs.dropdown.style.width = this.width + "px";     
      }
    },

    left: function () {
      try {
        let me = this;
        if (me.left == 0) {
          this.hideListDropdown = true;
        } else {
          this.hideListDropdown = false;
          this.$refs.dropdown.style.left = this.left - 158 + "px";                  
        }
      } catch (err) {
        console.log(err);
      }
    },
  },
};
</script>

<style></style>
