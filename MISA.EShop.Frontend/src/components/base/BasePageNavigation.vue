<template>
  <div class="page-navigator">
    <!-- <div class="ml-10" id="div1-paging"></div> -->
    <div class="paging">
      <div
        class="btn common-page first-page"
        @click="pageNumberOnClick('first-page')"
      ></div>
      <div
        class="btn common-page prev-page"
        @click="pageNumberOnClick('prev-page')"
      ></div>
      &nbsp;Trang&nbsp;&nbsp;
      <div       
        class="btn page-number"
      >
        {{ currentPageNumber }}
      </div>
      &nbsp;trên {{totalPageNumber}}&nbsp;&nbsp;
      <div
        class="btn common-page next-page"
        @click="pageNumberOnClick('next-page')"
      ></div>
      <div
        class="btn common-page last-page"
        @click="pageNumberOnClick('last-page')"
      ></div>
      <div
        class="btn common-page reload"
        @click="pageNumberOnClick('reload')"
      ></div>
      <div class="btn entity-perpage">
        <div @click="showOption()" class = "d-flex">
          <div class="">{{entityPerPage}}</div>
          <div class="arrow"></div>
        </div>
        <div :class="['quantity-option', {'d-none': hideOption}]">
          <div @click="chooseQuantity(15)" class="quantity-item">15</div>
          <div @click="chooseQuantity(25)" class="quantity-item">25</div>
          <div @click="chooseQuantity(50)" class="quantity-item">50</div>
          <div @click="chooseQuantity(100)" class="quantity-item">100</div>
        </div>
        <div class="">

        </div>
      </div>
    </div>
    <div class="ml-10">
      Hiển thị 1 - {{ realEntityPerPage }} trên {{ totalEntity }} kết quả
    </div>
  </div>
</template>

<script>
export default {
  name: "BasePageNavigation",
  props: {
    totalEntity: Number,
    totalPageNumber: Number,
    searchContent: String,
    realEntityPerPage: Number,
  },

  data() {
    return {
      // Trang hiện tại
      currentPageNumber: 1,
      entityPerPage: 50,
      hideOption: true,
    };
  },

  methods: {
    /**
     * Hàm mở chọn pageSize
     * Created By: Ngọc 27/09/2021
     */
    showOption(){
      this.hideOption = !this.hideOption;
    },

     /**
     * Hàm chọn số lượng bản ghi trên 1 trang
     * Created By: Ngọc 27/09/2021
     */
    chooseQuantity(entityPerPage){
      this.entityPerPage = entityPerPage;
       this.hideOption = true;
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
          me.currentPageNumber = btnPage;
          break;
      }
      me.updatePage();
    },

    /**
     * Hàm cập nhật trang đang được xem
     * Ngọc 12/8/2021
     */
    updateCenterNumber() {
      let me = this;
      me.currentPageNumber = Math.min(me.currentPageNumber, me.totalPageNumber);
      me.lowerLimit = me.upperLimit = me.currentPageNumber;
      for (var b = 1; b < me.totalShow && b < me.totalPageNumber; ) {
        if (me.lowerLimit > 1) {
          me.lowerLimit--;
          b++;
        }
        if (b < me.totalShow && me.upperLimit < me.totalPageNumber) {
          me.upperLimit++;
          b++;
        }
      }
    },

    /**
     * Hàm tăng/giảm số lượng thực thể theo trang
     * Ngọc 12/8/2021
     */
    modifyNumber(modifyStatus) {
      let me = this;
      me.$emit("modifyNumber", modifyStatus);

      me.updatePage();
    },

    /**
     * Hàm gọi lên hàm update ở cha
     * Ngọc 22/8/2021
     */
    updatePage() {
      let me = this;
      me.$emit("updatePage", me.currentPageNumber);
      me.updateCenterNumber();
    },
  },

  created() {
    //this.updatePage();
  },

  watch: {
    currentPageNumber: function () {
      this.updatePage();
    },

    totalPageNumber: function () {
      this.updatePage();
    },

    totalEntity: function () {
      this.updatePage();
    },
  },
};
</script>

<style></style>