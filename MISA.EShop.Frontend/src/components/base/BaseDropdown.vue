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
import ResourceVN from "../../common/resourceVN.js";
import Enumeration from "../../common/enumeration.js";

export default {
  name: "BaseDropdown",

  components: {},

  props: {
    itemId: String,
    itemName: String,
    left: Number,
    width: Number,
    count: Number,
  },

  data() {
    return {
      hideListDropdown: true,
      items: [],
      checkPos: 0,
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
                Operator: Enumeration.OperatorWord.Contain,
                OperatorName: "* : Chứa",
              },
              {
                Operator: Enumeration.OperatorWord.Equal,
                OperatorName: "= : Bằng",
              },
              {
                Operator: Enumeration.OperatorWord.StartWith,
                OperatorName: "+ : Bắt đầu bằng",
              },
              {
                Operator: Enumeration.OperatorWord.EndWith,
                OperatorName: "- : Kết thúc bằng",
              },
              {
                Operator: Enumeration.OperatorWord.NotContain,
                OperatorName: "! : Không chứa",
              },
            ];
            break;

          case "FilterPriceName":
            this.items = [
              {
                FilterPrice: Enumeration.OperatorNumber.Equal,
                FilterPriceName: "= : Bằng",
              },
              {
                FilterPrice: Enumeration.OperatorNumber.Lower,
                FilterPriceName: "< : Nhỏ hơn",
              },
              {
                FilterPrice: Enumeration.OperatorNumber.LE,
                FilterPriceName: `≤ : Nhỏ hơn hoặc bằng`,
              },
              {
                FilterPrice:  Enumeration.OperatorNumber.Greater,
                FilterPriceName: "> : Lớn hơn",
              },
              {
                FilterPrice:  Enumeration.OperatorNumber.GE,
                FilterPriceName: "≥ : Lớn hơn hoặc bằng",
              },
            ];
            break;

          case "Display":
            this.items = [
              {
                DisplayId: Enumeration.Display.No,
                Display: ResourceVN.Display.No,
              },
              {
                DisplayId: Enumeration.Display.Yes,
                Display: ResourceVN.Display.Yes,
              },
              {
                DisplayId: Enumeration.Display.All,
                Display: ResourceVN.Display.All,
              },
            ];
            break;

          case "StatusBusiness":
            this.items = [
              {
                StatusBusinessId: Enumeration.StatusBusiness.StopTrading,
                StatusBusiness: ResourceVN.StatusBusiness.StopTrading,
              },
              {
                StatusBusinessId: Enumeration.StatusBusiness.Trading,
                StatusBusiness: ResourceVN.StatusBusiness.Trading,
              },
              {
                StatusBusinessId: Enumeration.StatusBusiness.All,
                StatusBusiness: ResourceVN.StatusBusiness.All,
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
      this.$emit("chooseDropdownItem", 0, index);
    },

    /**
     * Hàm reset vị trí của dropdown về 0
     * Created By: Ngọc 30/08/2021
     */
    resetPos(checkPos) {
      this.$emit("resetPos", checkPos);
    },
  },

  watch: {
    itemName: function () {
      this.setValue();
      if (
        this.itemName == "FilterPriceName" ||
        this.itemName == "OperatorName"
      ) {
        this.$refs.dropdown.style.width = "auto";
      } else {
        this.$refs.dropdown.style.width = this.width + "px";
      }
    },

    // left: function () {
    //   try {
    //     let me = this;
    //     if (me.left == 0) {
    //       this.hideListDropdown = true;
    //     } else {
    //       this.hideListDropdown = false;
    //       this.$refs.dropdown.style.left = this.left - 158 + "px";
    //     }
    //   } catch (err) {
    //     console.log(err);
    //   }
    // },

    count: function () {
      try {
        let me = this;
        if (me.left == me.checkPos || me.left == 0) {
          me.checkPos = 0;
          this.hideListDropdown = true;
          me.resetPos(me.checkPos);
        } else {
          me.checkPos = me.left;
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
