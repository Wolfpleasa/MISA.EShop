<template>
  <div class="eshop-attr">
    <div :class="[{ 'd-none': !labelText.length > 0 }]">
      {{ labelText }}
    </div>
    <div class="attribute">
      <div class="attribute-name">{{ attributeName }}</div>
      <ul class="color-list" v-on:keydown="addToColorList($event)">
        <li class="color-item" v-for="color in colors" :key="color">
          {{color}}
          <div class="icon-cancel-white" @click="deleteAttribute(color)"></div>
        </li>
        <input
          :tabindex="tabindex"
          :type="type"
          :placeholder="placeholder"
          :FieldName="FieldName"
          :maxlength="maxlength"
          :class="['textbox-attribute', subClass]"
          ref="myColor"
          v-model="inputValue"
        />
      </ul>
    </div>
  </div>
</template>

<script>
import { mixin as clickaway } from "vue-clickaway";

export default {
  mixins: [clickaway],
  name: "BaseAtribute",
  components: {},

  model: {
    prop: 'colors', // ko cần watch colors vì đã có v-model ở cha wath hộ
    event: 'input' // tên sự kiện, chỉ cần emit input là colors của cha sẽ thay đổi
  },

  props: {
    labelText: String,
    tabindex: String,
    type: String,
    placeholder: String,
    FieldName: String,
    subClass: String,
    // Độ dài tối đa
    maxlength: String,
    attributeName: String,
    colors: {
      type: Array,
      default: () => []  
    }
  },
  data() {
    return {
      // Giá trị/Nội dung hiện tại
      inputValue: "",
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện nút enter để thêm màu vào danh sách
     * Created By: Ngọc 23/09/2021
     */
    addToColorList(event) {
      try {
        if ((event.code == "Enter" || event.code == "Tab") && this.inputValue!="") {
          event.preventDefault();
          var color = this.inputValue.trim();
          var colors = [];
          if (this.colors) {
            colors = [...this.colors, color];
          }
          else {
            colors = [color];
          }
          // Thêm vào danh sách màu
          this.$emit("input", colors);
          // Thêm vào bảng
          this.$emit("addColorToTableDetail", color);
          this.inputValue = "";
        }
      } catch (err) {
        console.log(err);
      }
    },

    deleteAttribute(color) {
      this.$emit('delete', color);
    }
  },
  watch: {
    
  },
};
</script>

<style scoped></style>
