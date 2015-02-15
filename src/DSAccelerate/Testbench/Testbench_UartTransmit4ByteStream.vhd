--------------------------------------------------------------------------------
-- Company: 
-- Engineer:
--
-- Create Date:   14:27:18 02/15/2015
-- Design Name:   
-- Module Name:   D:/jun/Research/git/DesignScriptCore/src/DSAccelerate/Testbench/Testbench_UartTransmit4ByteStream.vhd
-- Project Name:  SynthesizedProgram
-- Target Device:  
-- Tool versions:  
-- Description:   
-- 
-- VHDL Test Bench Created by ISE for module: UartTransmit4Test
-- 
-- Dependencies:
-- 
-- Revision:
-- Revision 0.01 - File Created
-- Additional Comments:
--
-- Notes: 
-- This testbench has been automatically generated using types std_logic and
-- std_logic_vector for the ports of the unit under test.  Xilinx recommends
-- that these types always be used for the top-level I/O of a design in order
-- to guarantee that the testbench will bind correctly to the post-implementation 
-- simulation model.
--------------------------------------------------------------------------------
LIBRARY ieee;
USE ieee.std_logic_1164.ALL;
 
-- Uncomment the following library declaration if using
-- arithmetic functions with Signed or Unsigned values
--USE ieee.numeric_std.ALL;
 
ENTITY Testbench_UartTransmit4ByteStream IS
END Testbench_UartTransmit4ByteStream;
 
ARCHITECTURE behavior OF Testbench_UartTransmit4ByteStream IS 
 
    -- Component Declaration for the Unit Under Test (UUT)
 
    COMPONENT UartTransmit4Test
    PORT(
         clock : IN  std_logic;
         reset : IN  std_logic;
         RS232_dataout : OUT  std_logic
        );
    END COMPONENT;
    

   --Inputs
   signal clock : std_logic := '0';
   signal reset : std_logic := '1';

 	--Outputs
   signal RS232_dataout : std_logic;

   -- Clock period definitions
   constant clock_period : time := 10 ns;
 
BEGIN
 
	-- Instantiate the Unit Under Test (UUT)
   uut: UartTransmit4Test PORT MAP (
          clock => clock,
          reset => reset,
          RS232_dataout => RS232_dataout
        );

  -- Stimulus process
   stim_proc: process
   begin		
	
		reset <= '1';
		wait for 5 ns;		
		
		-- Begin execution
		reset <= '0';
		wait for 5 ns;
		
		for a in 1 to 10000000 loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
		reset <= '1';
		wait for 5 ns;		
		
		-- Begin execution
		reset <= '0';
		wait for 5 ns;
		
		for a in 1 to 10000000 loop
			clock <= not clock;
			wait for 5 ns;
		end loop;
		
      wait;
   end process;

END;
