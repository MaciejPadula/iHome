<script lang="ts">
    import { NavBrand, NavHamburger, NavLi, NavUl, Navbar } from "flowbite-svelte";
    import UserControls from "./UserControls.svelte";

</script>
<Navbar color="form">
  <NavBrand href="/">
    <span class="self-center whitespace-nowrap text-xl font-semibold dark:text-white">iHome</span>
  </NavBrand>
  <div class="flex">
    <UserControls />
  </div>
</Navbar>