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

      <!-- <div class="image-contain"></div> -->

      <img :src="image" alt="" width="198px" height="122px">

      <div :class="['btn-image', {'d-flex': image != urlDefault}]">
        <ButtonIcon tabindex="0" @btn-click="choosePicture" buttonText="..." />
        <div @click="removePicture" v-if="image != urlDefault" class="icon-cancel"></div>
      </div>
      <input type="file" ref="myProduct" class="d-none" @change="chooseFile($event)">
    </div>

    <div class = "note">
      <div>- Lựa chọn biểu tượng để thay thế nếu không có ảnh</div><br>
      <div>- Định dạng ảnh (.jpg, .jpeg, .png, .gif) và dung lượng &lt; 5MB</div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import ButtonIcon from "./BaseButtonIcon.vue";
import Constant from '../../common/constant1';
export default {
  name: "BasePicture",
  components: {
    ButtonIcon,
  },

  props: {
    labelText: String,
    tabindex: String,
    pictureId: String,
  },
  data() {
    return {
      items: [],
      currentIndex: 0,
      image: "",
      urlDefault:"https://localhost:44389/images/products/bdebe2cb-bccb-413e-aa18-089372975423.png",
    };
  },

  methods: {
    /**
     * Hàm chọn ảnh từ file
     * Created By: Ngọc 05/10/2021
     */
    choosePicture(){
      this.$refs.myProduct.click();
    },

    /**
     * Hàm chọn ảnh từ file
     * Created By: Ngọc 05/10/2021
     */
    chooseFile(e){
      this.image = URL.createObjectURL(e.target.files[0]);
      this.$emit('chooseFile' ,e.target.files[0]);
    },

    /**
     * xóa ảnh
     * Created By: Ngọc 05/10/2021
     */
    removePicture(){
      this.image = this.urlDefault;
      this.$emit('chooseFile' ,null);
    },

    /**
     * Lấy ảnh theo Id
     * Created By: Ngọc 05/10/2021
     */
    getPictureInfo(){
      let me = this;
      axios.get(`${Constant.LocalUrl}/Pictures/${me.pictureId}`)
          .then(res => {
            me.items = res.data;
            me.image = `https://localhost:44389/images/products/${me.pictureId}${me.items.PictureTail}`;
          }).catch(err => {
            console.log(err);
          })
    }
  },

  watch: {    
    pictureId: function() {
      if(this.pictureId == "") this.image = this.urlDefault;
      else this.getPictureInfo();
    },
  },

  created() {
    this.image = this.urlDefault;
  },
};
</script>

<style scoped></style>
