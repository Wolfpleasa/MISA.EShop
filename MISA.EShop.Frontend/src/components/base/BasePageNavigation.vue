<template>
  <div class="page-navigator">
    <!-- <div class="ml-10" id="div1-paging"></div> -->
    <div class="paging">
      <div class="p-relative tt-field">
        <div
          :class="[
            'btn common-page first-page',
            { disable: currentPageNumber == 1 },
          ]"
          @click="pageNumberOnClick('first-page')"
        ></div>
        <ToolTip toolTipText="Trang đầu" :hideToolTip="hideToolTip" />
      </div>

      <div class="p-relative tt-field">
        <div
          :class="[
            'btn common-page prev-page',
            { disable: currentPageNumber == 1 },
          ]"
          @click="pageNumberOnClick('prev-page')"
        ></div>

        <ToolTip toolTipText="Trang trước" :hideToolTip="hideToolTip" />
      </div>

      &nbsp;Trang&nbsp;&nbsp;

      <input
        class="btn page-number"
        type="text"
        v-model="currentPageNumber"
        @change="changeCurrentPageNumber"
        @input="onInput($event.target.value)"
      />

      &nbsp;trên {{ totalPageNumber }}&nbsp;&nbsp;

      <div class="p-relative tt-field">
        <div
          :class="[
            'btn common-page next-page',
            { disable: currentPageNumber == totalPageNumber },
          ]"
          @click="pageNumberOnClick('next-page')"
        ></div>
        <ToolTip toolTipText="Trang sau" :hideToolTip="hideToolTip" />
      </div>

      <div class="p-relative tt-field">
        <div
          :class="[
            'btn common-page last-page',
            { disable: currentPageNumber == totalPageNumber },
          ]"
          @click="pageNumberOnClick('last-page')"
        ></div>
        <ToolTip toolTipText="Trang cuối" :hideToolTip="hideToolTip" />
      </div>

      <div class="p-relative tt-field">
        <div
          class="btn common-page reload"
          @click="pageNumberOnClick('reload')"
        ></div>
        <ToolTip toolTipText="Nạp" :hideToolTip="hideToolTip" />
      </div>

      <div class="btn entity-perpage">
        <div @click="showOption()" class="d-flex">
          <div class="">{{ entityPerPage }}</div>
          <div class="arrow"></div>
        </div>
        <div :class="['quantity-option', { 'd-none': hideOption }]">
          <div @click="chooseQuantity(15)" class="quantity-item">15</div>
          <div @click="chooseQuantity(25)" class="quantity-item">25</div>
          <div @click="chooseQuantity(50)" class="quantity-item">50</div>
          <div @click="chooseQuantity(100)" class="quantity-item">100</div>
        </div>
        <div class=""></div>
      </div>
    </div>
    <div class="ml-10">
      Hiển thị 1 - {{ realEntityPerPage }} trên {{ totalEntity }} kết quả
    </div>
  </div>
</template>

<script>
import ToolTip from "../../components/base/BaseToolTip1.vue";

export default {
  name: "BasePageNavigation",
  components: {
    ToolTip,
  },

  props: {
    totalEntity: Number,
    totalPageNumber: Number,
    searchContent: String,
    realEntityPerPage: Number,
    entityPerPage: {
      type: Number,
      default: 50,
    },
  },

  data() {
    return {
      // Trang hiện tại
      currentPageNumber: 1,
      hideOption: true,
      hideToolTip: true,
    };
  },

  methods: {
    /**
     * Hàm mở chọn pageSize
     * Created By: Ngọc 27/09/2021
     */
    showOption() {
      this.hideOption = !this.hideOption;
    },

    /**
     * Hàm chọn số lượng bản ghi trên 1 trang
     * Created By: Ngọc 27/09/2021
     */
    chooseQuantity(entityPerPage) {
      this.$emit("chooseQuantity", entityPerPage, this.currentPageNumber);
      this.hideOption = true;
    },

    onInput(inpValue) {
      let val = inpValue + "",
        number = "";
      for (let i = 0; i < val.length; i++) {
        if (!isNaN(val[i])) number += val[i];
      }

      this.currentPageNumber = number ? parseInt(number) + "" : "";
    },

    /**
     * Hàm bắt sự kiện nhập số trang
     * Created By: Ngọc 27/09/2021
     */
    changeCurrentPageNumber() {
      let me = this;
      if (me.currentPageNumber < 1) me.currentPageNumber = 1;
      else if (me.currentPageNumber > me.totalPageNumber)
        me.currentPageNumber = me.totalPageNumber;

      me.updatePage();
    },

    /**
     * Hàm dùng để chuyển trang
     * Ngọc 12/8/2021
     */
    pageNumberOnClick(btnPage) {
      let me = this;
      switch (btnPage) {
        case "first-page":
          me.currentPageNumber = 1;
          break;
        case "prev-page":
          if (me.currentPageNumber > 1) me.currentPageNumber -= 1;
          break;
        case "next-page":
          if (me.currentPageNumber < me.totalPageNumber)
            me.currentPageNumber += 1;
          break;
        case "last-page":
          me.currentPageNumber = me.totalPageNumber;
          break;
        default:
          break;
      }
      me.updatePage();
    },

    /**
     * Hàm gọi lên hàm update ở cha
     * Ngọc 22/8/2021
     */
    updatePage() {
      let me = this;
      me.$emit("updatePage", me.currentPageNumber);
    },
  },

  created() {
    this.updatePage();
  },

  watch: {
    // currentPageNumber: function () {
    //   this.updatePage();
    // },

    totalPageNumber: function () {
      let me = this;
      me.currentPageNumber = Math.min(me.currentPageNumber, me.totalPageNumber);
    },

    // totalEntity: function () {
    //   this.updatePage();
    // },
  },
};
</script>

<style></style>
