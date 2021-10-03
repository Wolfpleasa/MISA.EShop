<template>
  <div class="d-flex pd-8">
    <div v-if="labelText.length > 0">
      {{ labelText }} 
    </div>

    <div class="d-flex ml-40">
      <div
        v-for="(item, index) in items"
        :key="item[itemId]"
        :tabindex="tabindex"
        :Value="item[itemId]"
        v-on:keyup.space="chooseRadioItemByKey($event, index)"
        @click="chooseRadioItem(item[itemId], itemId, index)"
        :class="['radio-item', index == currentIndex ? 'selected' : '']"
      >
        <div class="radio"></div>
        <div class="text">{{ item[itemName] }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import ResourceVN from "../../common/resourceVN";
import Enumeration from "../../common/enumeration";
export default {
  name: "BaseInput",
  props: {
    labelText: String,
    tabindex: String,
    itemId: String,
    itemName: String,
    FieldName: String,
    selectedId: String,
  },
  data() {
    return {
      items: [],
      currentIndex: 0,
    };
  },

  methods: {
    /**
     * Sự kiện chọn 1 lựa chọn bằng phím
     * Created By: Ngọc 28/09/2021
     */
    chooseRadioItemByKey(event, index) {
      try {
        if (event.code == "Space") {
          event.preventDefault();
          let item = this.items[index],
            itemValue = item[this.itemId];
          this.chooseRadioItem(itemValue, this.itemId, index);
        }
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Hàm bắt sự kiện chọn giới tính
     * Created By: Ngọc 28/09/2021
     */
    chooseRadioItem(itemValue, itemId, index) {
      try {
        let me = this;
        me.currentIndex = index;
        me.$emit("chooseRadioItem", itemId, itemValue);
      } catch (err) {
        console.log(err);
      }
    },

    /**
     * Xem hàm sửa có gọi không để chọn giới tính tương ứng
     * Created By: Ngọc 28/09/2021
     */
    setValueRadio() {
      try {
        let me = this;
        // nếu selectedId(StatusBusiness) mà tồn tại(tức là được Employeelist truyền vào => form sửa dùng)
        if ((me.selectedId + "").length > 0) {
          // duyệt từng items(gender)
          me.items.forEach(function (item, index) {
            // nếu selectedId(StatusBusiness) từ cha truyền vào mà trùng với itemId(StatusBusiness) trong list
            if (me.selectedId == item[me.itemId]) {
              // gán currentIndex = index (để hàng tương ứng được tích)
              me.currentIndex = index;
            }
          });
        } else {
          // form thêm dùng
          // Tự chọn giới tính nam
          me.currentIndex = 0;
        }
      } catch (err) {
        console.log(err);
      }
    },
  },

  created() {
    let me = this;
    switch (me.itemName) {
      case "StatusBusinessName":
        this.items = [
          {
            StatusBusiness: Enumeration.StatusBusiness.Trading,
            StatusBusinessName: ResourceVN.StatusBusiness.Trading,
          },
          {
            StatusBusiness: Enumeration.StatusBusiness.StopTrading,
            StatusBusinessName: ResourceVN.StatusBusiness.StopTrading,
          },      
        ];
        break;

      default:
        break;
    }
  },

  watch: {
    selectedId: function () {
      this.setValueRadio();
    },
  },
};
</script>

<style scoped>
</style>
