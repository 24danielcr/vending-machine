<template>
  <div class="container">
    <h1 class="mb-4">Vending Machine</h1>

    <div
      class="card mb-3"
      v-for="(product, index) in products"
      :key="index"
    >
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-center">
          <div>
            <h5 class="card-title">{{ product.name }}</h5>

            <p class="card-text text-muted" type="number">
              Price: ¢{{ product.price }}
            </p>

            <p class="card-text text-muted">
              Available: {{ product.quantity }}
            </p>

          </div>
          <input
            type="number"
            class="form-control w-25"
            v-model.number="selectedProducts[index].quantity"
            @input="validateQuantity(index)"
            min="0"
          />
        </div>
        <div v-if="productErrors[index]" class="text-danger mt-2">
          {{ productErrors[index] }}
        </div>
      </div>
    </div>

    <div v-if="!disableBuy && !outOfService">
      <h5 class="mt-3">
        Total to pay: ¢{{ selectedProductsTotal }}
      </h5>
    </div>

    <h3 class="mt-4">Insert Money</h3>

    <div
      class="row align-items-center mb-2"
      v-for="(coin, index) in selectedCoins"
      :key="coin.value"
    >
      <div class="col-auto" style="width: 80px;">
        <label class="mb-0">¢{{ coin.value }}:</label>
      </div>
      <div class="col">
        <input
          type="number"
          class="form-control form-control"
          style="width: 80px;"
          min="0"
          v-model.number="selectedCoins[index].quantity"
        />
      </div>
    </div>

    <div v-if="!disableBuy && !outOfService">
      <h5 class="mt-3">
        Inserted money: ¢{{ insertedMoneyTotal }}
      </h5>
    </div>

    <div v-if="displayChange" class="card mt-4 mb-4">
      <div class="card-body">
        <div class="d-flex justify-content-between align-items-start">
          <div>
            <h5 class="card-title">Your change is of {{ changeTotal }} colones.</h5>
            <div v-if="changeTotal > 0">
              <p class="card-text text-muted mb-2">Breakdown:</p>
              <ul class="mb-0">
                <li v-for="(coin, index) in change" :key="index">
                  {{ coin.quantity }} coins of ¢{{ coin.value }}
                </li>
              </ul>
            </div>
          </div>
          <button
            class="btn-close"
            aria-label="Close"
            @click="displayChange = false"
          ></button>
        </div>
      </div>
    </div>


    <div v-if="errorOn" class="text-danger mt-2">
      {{ errorMsg }}
    </div>

    <div v-if="disableBuy && !outOfService" class="text-danger mt-2">
      Fix order before buying.
    </div>

    <div v-if="outOfService" class="text-danger mt-2">
      Out of Service.
    </div>

    <button class="btn btn-primary mb-1 mt-1" @click="handlePurchase" :disabled="disableBuy || outOfService">
      Purchase
    </button>

  </div>
</template>

<script>
import axios from 'axios'

export default {
  name: 'VendingMachine',
  data() {
    return {
      products: [],
      coins: [],
      selectedProducts: [],
      selectedCoins: [
      { value: 1000, quantity: 0},
      { value: 500, quantity: 0 },
      { value: 100, quantity: 0 },
      { value: 50, quantity: 0 },
      { value: 25, quantity: 0 }
    ],
      productErrors: [],
      coinsTotal: 0,
      disableBuy: false,
      outOfService: false,
      displayChange: false,
      change: [],
      changeTotal: 0,
      errorMsg: "",
      errorOn: false,
    }
  },
  methods: {
    async getProducts() {
      try {
        const response = await axios.get('https://localhost:7254/api/Products');
        this.products = response.data;
        this.selectedProducts = this.products.map(product => ({
          name: product.name,
          quantity: 0
        }));

        this.productErrors = this.products.map(() => '');
      } catch (error) {
        this.errorMsg = 'Failed to get products. ';
        this.errorOn = true;
      }
    },
    async getCoins() {
      try {
        const response = await axios.get('https://localhost:7254/api/Coins');
        this.coins = response.data;
        this.selectedCoins.forEach(coin => {
          coin.quantity = 0;
        });

        this.coinsTotal = this.coins.reduce((sum, coin) => {
          return sum + coin.quantity;
        }, 0);
        if (this.coinsTotal === 0) {
          this.outOfService = true;
        }

      } catch(error) {
        this.errorMsg += 'Failed to get coins. ';
        this.errorOn = true;
      }
    },
    validateQuantity() {
      let hasError = false;

      this.products.forEach((product, index) => {
        const selected = this.selectedProducts[index].quantity;
        const available = product.quantity;

        if (selected > available) {
          this.productErrors[index] = `Only ${available} available`;
          hasError = true;
        } else {
          this.productErrors[index] = '';
        }
      });

      this.disableBuy = hasError;
    },
    async handlePurchase() {
      this.displayChange = false;
      this.errorOn = false;
      this.errorMsg = "";
      const orderProducts = this.selectedProducts
        .map((product) => ({
          name: product.name,
          quantity: product.quantity
        }))
        .filter(item => item.quantity > 0);

      const orderPayment = this.selectedCoins
        .map((coin) => ({
          value: coin.value,
          quantity: coin.quantity
        }))
        .filter(item => item.quantity > 0);

      if (orderProducts.length === 0) {
        this.errorOn = true;
        this.errorMsg = "Select a product before purchasing.";
        return;
      }

      try {
        const response = await axios.post('https://localhost:7254/api/Payment', {
          coinsPayment: orderPayment,
          products: orderProducts
        });

        this.change = response.data;

          this.changeTotal = this.change.reduce((sum, coin) => {
            return sum + (coin.quantity * coin.value);
          }, 0);
          this.displayChange = true;

          this.updateVendingMachine();
      } catch (error) {
        this.errorOn = true;
        this.errorMsg = "Failed to make purchase.";
      }
    },
    updateVendingMachine() {
      this.errorMsg = "";
      this.errorOn = false
      this.getProducts();
      this.getCoins();
    }

  },
  mounted() {
    this.updateVendingMachine();
  },
  computed: {
    selectedProductsTotal() {
      return this.selectedProducts.reduce((total, selected, index) => {
        const product = this.products[index];
        if (!product) return total;
        return total + selected.quantity * product.price;
      }, 0);
    },
    insertedMoneyTotal() {
      return this.selectedCoins.reduce((total, selected) => {
        return total + selected.quantity * selected.value;
      }, 0);
    }
  }
}
</script>

<style scoped>
</style>
