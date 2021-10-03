<template>
  <div class="d-flex pd-8">
    <div v-if="labelText.length > 0">
      {{ labelText }}
    </div>

    <div class="image">
      <div class="icon-product">
        <div class="iconic"></div>
        <div>Biểu tượng</div>
      </div>
      <div class="image-contain">
        <div class="img-product"></div>
      </div>
      <div class="btn-image">
        <ButtonIcon buttonText="..." />
      </div>
    </div>

    <div class = "note">
      <div>- Lựa chọn biểu tượng để thay thế nếu không có ảnh</div><br>
      <div>- Định dạng ảnh (.jpg, .jpeg, .png, .gif) và dung lượng &lt; 5MB</div>
    </div>
  </div>
</template>

<script>
import ButtonIcon from "./BaseButtonIcon.vue";
export default {
  name: "BaseInput",
  components: {
    ButtonIcon,
  },

  props: {
    labelText: String,
    tabindex: String,
  },
  data() {
    return {
      items: [],
      currentIndex: 0,
    };
  },

  methods: {},

  watch: {
    selectedId: function() {
      this.setValueRadio();
    },
  },
};
</script>

<style scoped></style>
